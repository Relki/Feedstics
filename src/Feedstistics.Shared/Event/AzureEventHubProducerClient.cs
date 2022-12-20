namespace Feedstistics.Shared.Event
{
    using Azure.Messaging.EventHubs;
    using Azure.Messaging.EventHubs.Producer;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Azure Event Hub Producer Client.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AzureEventHubProducerClient<T> : IEventProducerClient<T>, IDisposable
    {
        private readonly ILogger<AzureEventHubProducerClient<T>> _logger;
        private readonly AzureEventHubProducerClientSettings _settings;
        private EventHubProducerClient _eventHubProducerClient = null;

        /// <inheritdoc/>
        public event EventHandler<Exception>? ExceptionOccured;

        /// <summary>
        /// Instansiates an instance of <see cref="AzureEventHubProducerClient"/>.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <exception cref="ArgumentNullException">AzureEventHubProducerClientSettings must be defined.</exception>
        public AzureEventHubProducerClient(
            IOptions<AzureEventHubProducerClientSettings> settings,
            ILoggerFactory loggerFactory)
        {
            if (settings is null || settings.Value is null)
            {
                throw new ArgumentNullException(nameof(settings), "AzureEventHubProducerClientSettings not defined. Include settings in configuration file.");
            }

            this._settings = settings.Value;
            this._logger = loggerFactory.CreateLogger<AzureEventHubProducerClient<T>>();
            this.InitializeClient();
        }

        /// <inheritdoc/>
        public async Task<bool> SendEvents(IEnumerable<T> messages, bool? continueOnException = false, CancellationToken cancellationToken = default)
        {
            var sendSuccesses = new List<bool>();

            // Create a batch of events 
            if (this._eventHubProducerClient.IsClosed)
            {
                this.InitializeClient();
            }

            using EventDataBatch eventBatch = await this._eventHubProducerClient.CreateBatchAsync(cancellationToken);

            int numOfEvents = 0;

            foreach (var message in messages)
            {
                numOfEvents += 1;
                string messageSerialized = JsonConvert.SerializeObject(messages);
                if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(messageSerialized))))
                {
                    var exceptionMessage = new Exception($"Event {messageSerialized} is too large for the batch and cannot be sent.");
                    // if it is too large for the batch
                    if (continueOnException is not null && continueOnException.Value)
                    {
                        this.OnExceptionOccured(exceptionMessage);
                        this._logger.LogWarning(exceptionMessage, $"Message too large for event hub. Message size: [{messageSerialized.Length}] bytes.");
                        sendSuccesses.Add(false);
                        continue;
                    }
                    else
                    {
                        throw exceptionMessage;
                    }
                }

                sendSuccesses.Add(true);
            }

            try
            {
                // Use the producer client to send the batch of events to the event hub
                //await _eventHubProducerClient.SendAsync(eventBatch, cancellationToken);
                await _eventHubProducerClient.SendAsync(eventBatch, cancellationToken);
                this._logger.LogDebug($"A batch of {numOfEvents} events has been published.");

                return sendSuccesses.All(success => success);
            }
            catch(Exception ex)
            {
                if (continueOnException is not null && continueOnException.Value)
                {
                    var exeptionMessage = $"Exception publishing [{numOfEvents}] messages batch to Azure Event Hub.";
                    this._logger.LogWarning(ex, exeptionMessage);
                    this.OnExceptionOccured(new Exception(exeptionMessage, ex));
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <inheritdoc/>
        public async Task<bool> SendEvent(T message, bool? continueOnException = false, CancellationToken cancellationToken = default)
        {
            var sendSuccesses = new List<bool>();

            // Create a batch of events
            if (this._eventHubProducerClient.IsClosed)
            {
                this.InitializeClient();
            }

            using EventDataBatch eventBatch = await this._eventHubProducerClient.CreateBatchAsync(cancellationToken);

            string messageSerialized = JsonConvert.SerializeObject(message);
            if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(messageSerialized))))
            {
                var exceptionMessage = $"Message too large for event hub. Message size: [{messageSerialized.Length}] bytes.";
                var exception = new Exception(exceptionMessage);
                // if it is too large for the batch
                if (continueOnException is not null && continueOnException.Value)
                {
                    this.OnExceptionOccured(exception);
                    this._logger.LogWarning(exception, exceptionMessage);
                    sendSuccesses.Add(false);
                }
                else
                {
                    throw exception;
                }
            }

            try
            {
                // Use the producer client to send the batch of events to the event hub
                await _eventHubProducerClient.SendAsync(eventBatch, cancellationToken);

                this._logger.LogInformation($"A batch of 1 events has been published.");

                return true;
            }
            catch (Exception ex)
            {
                if (continueOnException is not null && continueOnException.Value)
                {
                    var exceptionMessage = $"Exception publishing [1] message batch to Azure Event Hub.";
                    this._logger.LogWarning(ex, exceptionMessage);
                    this.OnExceptionOccured(new Exception(exceptionMessage, ex));
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this._eventHubProducerClient != null)
            {
                Task.Run(() => this._eventHubProducerClient.DisposeAsync());
            }

            GC.SuppressFinalize(this);
        }

        private void InitializeClient()
        {
            this._eventHubProducerClient = new EventHubProducerClient(_settings.EventHubConnectionString, _settings.EventHubName);
        }

        private void OnExceptionOccured(Exception exception)
        {
            var exceptionOccuredEvent = this.ExceptionOccured;
            if (exceptionOccuredEvent != null)
            {
                this.OnExceptionOccured(exception);
            }
        }
    }
}
