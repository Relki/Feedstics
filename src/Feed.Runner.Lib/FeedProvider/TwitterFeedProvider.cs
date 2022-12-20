namespace Feed.Runner.Lib.FeedProvider
{
    using Feedstistics.Shared;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using Twitter.Api.Client;
    using Twitter.Api.Data;

    /// <summary>
    /// Twitter Feed Provider.
    /// </summary>
    public class TwitterFeedProvider : IFeedProvider
    {
        private readonly ILogger<TwitterFeedProvider> _logger;
        private readonly TwitterFilteredStreamApiClient _twitterFilteredStreamApiClient;
        private bool _isRunning = false;
        private bool _stopRequested = false;

        /// <inheritdoc/>
        public event EventHandler<FeedStartedEventArgs>? Feed_Started;

        /// <inheritdoc/>
        public event EventHandler<FeedEndedEventArgs>? Feed_Ended;

        /// <inheritdoc/>
        public event EventHandler<FeedUpdateEventArgs>? Feed_Update;

        /// <inheritdoc/>
        public string FeedProviderName { get; set; } = "Twitter";

        /// <summary>
        /// Instansiates an instance of <see cref="TwitterFeedProvider"/>.
        /// </summary>
        /// <param name="twitterFilteredStreamApiClient">TwitterFilteredStreamApiClient for streaming filtered tweets.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        /// <param name="settings">Settings.</param>
        public TwitterFeedProvider(
                TwitterFilteredStreamApiClient twitterFilteredStreamApiClient,
                ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<TwitterFeedProvider>();
            this._twitterFilteredStreamApiClient = twitterFilteredStreamApiClient;

            this._twitterFilteredStreamApiClient.FilteredStream_TweetsReceived += this.OnFeedUpdate;
            this._twitterFilteredStreamApiClient.FilteredStream_Started += this.OnFeedStarted;
            this._twitterFilteredStreamApiClient.FilteredStream_Ended += this.OnFeedEnded;
        }

        /// <inheritdoc/>
        public async Task Start(CancellationToken? cancellationToken = default!)
        {
            this._stopRequested = false;
            await this.StartListenerAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> Stop(CancellationToken? cancellationToken = default!)
        {
            // Add timer
            this._stopRequested = true;
            while (this._isRunning)
            {
                if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
                {
                    this.OnFeedEnded("Stop requested but was aborted.");
                    return false;
                }

                await Task.Delay(200);
            }

            this.OnFeedEnded("Stop Requested");

            return !this._isRunning;
        }

        /// <inheritdoc/>
        public IFeedstisticsMessage ToFeedStatisticMessage(object message)
        {
            if (message is not Tweet tweet)
            {
                throw new InvalidOperationException($"{nameof(message)} is not of type [Tweet]");
            }

            return new TweetMessageType(tweet);
        }

        /// <summary>
        /// Start listing to feed provider.
        /// </summary>
        /// <param name="cancellationToken">Optional: CancellationToken.</param>
        /// <returns>Task.</returns>
        private async Task StartListenerAsync(CancellationToken? cancellationToken = default!)
        {
            this._isRunning = true;

            while ((cancellationToken.HasValue && !cancellationToken.Value.IsCancellationRequested) && !this._stopRequested)
            {
                await this._twitterFilteredStreamApiClient.StartTweetStreamer(cancellationtoken: cancellationToken);
            }

            this._isRunning = false;
        }

        protected virtual void OnFeedStarted(object? sender, StreamStartedEventArgs e)
        {
            this._logger.LogDebug($"OnFeedEnded fired for at [{e.StreamStartedStartTimeUtc}] for provider [{this.FeedProviderName}]");
            var eventArgs = new FeedStartedEventArgs(this.FeedProviderName, e.StreamStartedStartTimeUtc);

            // Event listers may stop listening between evaluating and invoking.
            var feedStarted = this.Feed_Started;
            if (feedStarted != null)
            {
                feedStarted?.Invoke(this, eventArgs);
            }
        }

        protected virtual void OnFeedEnded(string stopReadon)
        {
            var nowUtc = DateTime.UtcNow;
            this._logger.LogDebug($"OnFeedEnded fired for at [{nowUtc}], Reason: [{stopReadon}] for provider [{this.FeedProviderName}]");
            var eventArgs = new FeedEndedEventArgs(this.FeedProviderName, nowUtc, stopReadon);

            // Event listers may stop listening between evaluating and invoking.
            var feedEnded = this.Feed_Ended;
            if (feedEnded != null)
            {
                feedEnded?.Invoke(this, eventArgs);
            }
        }

        protected virtual void OnFeedEnded(object? sender, StreamEndedEventArgs e)
        {
            this._logger.LogDebug($"OnFeedEnded fired for at [{e.StoppedTimeUtc}], Reason: [{e.Reason}] for provider [{this.FeedProviderName}]");
            var eventArgs = new FeedEndedEventArgs(this.FeedProviderName, e.StoppedTimeUtc, e.Reason.ToString());

            // Event listers may stop listening between evaluating and invoking.
            var feedEnded = this.Feed_Ended;
            if (feedEnded != null)
            {
                feedEnded?.Invoke(this, eventArgs);
            }
        }

        protected virtual void OnFeedUpdate(object? sender, StreamFilteredTweetReceivedEventArgs e)
        {
            this._logger.LogDebug($"OnFeedUpdate fired for [{e.TweetBatch?.Length}] at [{e.BatchedDateTimeUtc}] for provider [{this.FeedProviderName}]");
            var feedstisticsMessages = e.TweetBatch == null ? new List<IFeedstisticsMessage>() : e.TweetBatch
                .Select(tweet => this.ToFeedStatisticMessage((object)tweet));

            var eventArgs = new FeedUpdateEventArgs(this.FeedProviderName, e.BatchedDateTimeUtc, feedstisticsMessages);

            // Event listers may stop listening between evaluating and invoking.
            var feedUpdate = this.Feed_Update;
            if (feedUpdate != null)
            {
                feedUpdate?.Invoke(this, eventArgs);
            }
        }
    }
}
