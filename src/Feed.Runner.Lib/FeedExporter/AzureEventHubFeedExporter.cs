namespace Feed.Runner.Lib.FeedExporter
{
    using Feedstistics.Shared;
    using Feedstistics.Shared.Event;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;

    /// <summary>
    /// Azure Event Hub Feed Exporter.
    /// </summary>
    public class AzureEventHubFeedExporter : IFeedExporter
    {
        private readonly ILogger<AzureEventHubFeedExporter> _logger;
        private readonly FeedExporterSettings _settings;
        private readonly IEventProducerClient<IFeedstisticsMessage> _eventHubProducerClient;

        /// <inheritdoc />
        public string FeedExporterName { get; set; } = "AzureEventHub";

        /// <summary>
        /// Inistaniates an instance of <see cref="AzureEventHubFeedExporter"/>.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <param name="eventHubProducerClient">EventHub Producer Client.</param>
        public AzureEventHubFeedExporter(
            IOptions<FeedExporterSettings> settings,
            ILoggerFactory loggerFactory,
            IEventProducerClient<IFeedstisticsMessage> eventHubProducerClient)
        {
            this._logger = loggerFactory.CreateLogger<AzureEventHubFeedExporter>();
            this._settings = settings.Value;
            this._eventHubProducerClient = eventHubProducerClient;

            this._eventHubProducerClient.ExceptionOccured += EventHubProducerClient_ExceptionOccured;
        }

        /// <inheritdoc />
        public async Task ExportFeed(IEnumerable<IFeedstisticsMessage> feedItems, CancellationToken cancellationToken = default)
        {
            if (feedItems == null || !feedItems.Any())
            {
                return;
            }

            if (this._eventHubProducerClient == null)
            {
                var ex = new InvalidOperationException($"IQueueClient not defined for feed exporter: [{this.FeedExporterName}]");
                this._logger.LogError(exception: ex, $"IQueueClient not defined for feed exporter: [{this.FeedExporterName}]");
                throw ex;
            }

            if (this._settings != null && this._settings.RequireIndividualItemExport)
            {
                foreach (var feedItem in feedItems)
                {
                    await this._eventHubProducerClient.SendEvent(feedItem, cancellationToken: cancellationToken);
                }

                return;
            }

            await this._eventHubProducerClient.SendEvents(feedItems, cancellationToken: cancellationToken);
        }

        private void EventHubProducerClient_ExceptionOccured(object? sender, Exception e)
        {
            this._logger.LogWarning($"Exception while publishing to event hub. Exception: [{e.InnerException}]");
        }
    }
}
