namespace Feedstistics.Shared.Queue
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Azure.Storage.Queues;
    using Azure.Storage.Queues.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    /// <summary>
    /// Azure Storage Queue client.
    /// </summary>
    public class AzureStorageQueueClient : IQueueClient
    {
        private readonly AzureStorageQueueClientSettings _settings;
        private readonly QueueClient _queueClient;
        private readonly ILogger<AzureStorageQueueClient> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureStorageQueueClient"/> class.
        /// </summary>
        /// <param name="client">Azure Queue Client.</param>
        /// <param name="logger">Logger.</param>
        public AzureStorageQueueClient(
            IOptions<AzureStorageQueueClientSettings> settings,
            ILogger<AzureStorageQueueClient> logger)
        {
            this._settings = settings.Value;
            this._queueClient = new QueueClient(this._settings.AzureQueueConnectionString, this._settings.AzureQueueName);
            this._logger = logger;
        }

        /// <summary>
        /// Convert to valid container name.
        /// </summary>
        /// <param name="containerName">Container name.</param>
        /// <returns>Valide container name.</returns>
        public static string ToURLSlug(string containerName)
        {
            return Regex.Replace(containerName, @"[^a-z0-9]+", "-", RegexOptions.IgnoreCase)
                .Trim(new char[] { '-' })
                .ToLower();
        }

        /// <inheritdoc/>
        public async Task<QueueMessage> DequeueMessageAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await this._queueClient.ReceiveMessageAsync(null, cancellationToken);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Exception occured while dequeing a message in azure queue.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task EnqueueMessageAsync(object message, CancellationToken cancellationToken = default)
        {
            try
            {
                if (await this._queueClient.CreateIfNotExistsAsync() != null)
                {
                    Console.WriteLine("The queue was created.");
                }

                string messageSerialized = JsonConvert.SerializeObject(message);
                await this._queueClient.SendMessageAsync(Base64Encode(messageSerialized), cancellationToken);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Exception occured while enqueing a message in azure queue.");
                throw;
            }
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
