namespace Feed.Runner.Lib
{
    using Microsoft.Extensions.Logging;
    using Feed.Runner.Lib.FeedExporter;
    using Feed.Runner.Lib.FeedProvider;
    using System.Threading;
    using Feedstistics.Shared;

    /// <summary>
    /// Manages feed providers and exports feed updates.
    /// </summary>
    public class FeedRunner : IFeedRunner
    {
        private readonly ILogger<FeedRunner> _logger = default!;
        private readonly IFeedProvider[] _feedProviders = default!;
        private readonly IFeedExporter[] _feedExporters = default!;

        /// <summary>
        /// Instansiates an instance of <see cref="FeedRunner"/>.
        /// </summary>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <param name="feedProviderFactory">Feed provider factory.</param>
        /// <param name="feedExporterFactory">Feed exporter that exports feed items.</param>
        public FeedRunner(
            ILoggerFactory loggerFactory,
            IFeedProviderFactory feedProviderFactory,
            IFeedExporterFactory feedExporterFactory)
        {
            this._logger = loggerFactory.CreateLogger<FeedRunner>();
            this._feedProviders = feedProviderFactory.GetFeedProviders(enabled: true) ?? Array.Empty<IFeedProvider>();
            this._feedExporters = feedExporterFactory.GetFeedExporters(enabled: true) ?? Array.Empty<IFeedExporter>();

            // for each feed provider hook up listeners.
            if (this._feedProviders != null)
            {
                foreach (var feedProvider in this._feedProviders)
                {
                    feedProvider.Feed_Update += this.FeedProvider_Feed_Update;
                    feedProvider.Feed_Started += this.FeedProvider_Feed_Started;
                    feedProvider.Feed_Ended += this.FeedProvider_Feed_Ended;
                }
            }
        }

        /// <inheritdoc/>
        public async Task StartFeeds(CancellationToken? cancellationToken = default!)
        {
            var feedProviderStartTasks = new List<Task>();
            foreach (var feedProvider in this._feedProviders)
            {
                try
                {
                    this._logger.LogInformation($"[{feedProvider.FeedProviderName}] feed provider starting...");

                    feedProviderStartTasks.Add(feedProvider.Start(cancellationToken));
                }
                catch (Exception ex)
                {
                    this._logger.LogError($"[{feedProvider.FeedProviderName}] feedItem reported exception while starting. Exception: {ex}");
                }
            }

            await Task.WhenAll(feedProviderStartTasks);
        }

        /// <inheritdoc/>
        public async Task<bool> StopFeeds(CancellationToken? cancellationToken = default!)
        {
            var feedProviderStopTasks = new List<Task<bool>>();

            foreach (var feedProvider in this._feedProviders)
            {
                try
                {
                    this._logger.LogInformation($"[{feedProvider.FeedProviderName}] feed provider stopping.");
                    feedProviderStopTasks.Add(feedProvider.Stop(cancellationToken));
                }
                catch (Exception ex)
                {
                    this._logger.LogError($"[{feedProvider.FeedProviderName}] feed provider reported exception while stopping. Exception: {ex}");
                }
            }

            bool[] stopResults = await Task.WhenAll(feedProviderStopTasks);
            return stopResults.All(stopResult => stopResult);
        }

        /// <inheritdoc/>
        public void ExportFeedItems(IEnumerable<IFeedstisticsMessage> feedItems)
        {
            if (this._feedExporters == null || !this._feedExporters.Any())
            {
                this._logger.LogWarning($"No feed exporters defined. Define dependency injection for IFeedExporter[]. [{feedItems.Count()}] Feed items not exported.");
                return;
            }
            
            if (feedItems != null)
            {
                foreach (var feedExporter in this._feedExporters)
                {
                    this._logger.LogDebug($"Exporting: [{feedItems.Count()}] feed items to [{feedExporter.FeedExporterName}] feed exporter.");

                    try
                    {
                        Task.Run(async () =>
                        {
                            try
                            {
                                await feedExporter.ExportFeed(feedItems);
                                this._logger.LogInformation($"Exported: [{feedItems.Count()}] feed items to [{feedExporter.FeedExporterName}] feed exporter.");
                            }
                            catch (Exception ex)
                            {
                                this._logger.LogWarning(ex, $"Exception occurred while exporting to export provider: [{feedExporter.FeedExporterName}]");
                                throw;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogError(ex, $"Failed to Export: [{feedItems.Count()}] feed items to[{feedExporter.FeedExporterName}] feed exporter.");
                        continue;
                    }
                }
            }
        }

        private void FeedProvider_Feed_Update(object? sender, FeedUpdateEventArgs e)
        {
            this._logger.LogInformation($"[{e.FeedItems.Count()}] updates from [{e.FeedSourceName}] feed provider.");
            this.ExportFeedItems(e.FeedItems.ToArray());
        }

        private void FeedProvider_Feed_Started(object? sender, FeedStartedEventArgs e)
        {
            this._logger.LogInformation($"[{e.FeedProviderName}] feed provider started.");
        }

        private void FeedProvider_Feed_Ended(object? sender, FeedEndedEventArgs e)
        {
            this._logger.LogInformation($"[{e.FeedProviderName}] feed provider Stopped. Reason: [{e.FeedEndReason}]");
        }
    }
}