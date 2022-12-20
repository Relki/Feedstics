namespace Feed.Runner.Lib.FeedExporter
{
    using Feedstistics.Shared;
    using Feedstistics.Shared.Queue;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;

    /// <summary>
    /// Azure Queue Feed Exporter.
    /// </summary>
    public class AzureQueueFeedExporter : IFeedExporter
    {
        private readonly ILogger<AzureQueueFeedExporter> _logger;
        private readonly FeedExporterSettings _settings;
        private readonly IQueueClient _queueClient;

        /// <inheritdoc />
        public string FeedExporterName { get; set; } = "AzureQueue";

        /// <summary>
        /// Inistaniates an instance of <see cref="AzureQueueFeedExporter"/>.
        /// </summary>
        /// <param name="settings">Settings.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <param name="queueClient">Queue Client.</param>
        public AzureQueueFeedExporter(
            IOptions<FeedExporterSettings> settings,
            ILoggerFactory loggerFactory,
            IQueueClient queueClient)
        {
            this._logger = loggerFactory.CreateLogger<AzureQueueFeedExporter>();
            this._settings = settings.Value;
            this._queueClient = queueClient;
        }

        /// <inheritdoc />
        public async Task ExportFeed(IEnumerable<IFeedstisticsMessage> feedItems, CancellationToken cancellationToken = default)
        {
            if (feedItems == null || !feedItems.Any())
            {
                return;
            }

            if (this._queueClient == null)
            {
                var ex = new InvalidOperationException($"IQueueClient not defined for feed exporter: [{this.FeedExporterName}]");
                this._logger.LogError(exception: ex, $"IQueueClient not defined for feed exporter: [{this.FeedExporterName}]");
                throw ex;
            }

            if (this._settings != null && this._settings.RequireIndividualItemExport)
            {
                foreach (var feedItem in feedItems)
                {
                    await this._queueClient.EnqueueMessageAsync(feedItem, cancellationToken);
                }

                return;
            }

            await this._queueClient.EnqueueMessageAsync(feedItems, cancellationToken);
        }
    }
}
