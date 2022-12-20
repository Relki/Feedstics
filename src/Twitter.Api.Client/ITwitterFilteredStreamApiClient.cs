namespace Twitter.Api.Client
{
    using Twitter.Api.Client.Data;
    using Twitter.Api.Data;

    /// <summary>
    /// Twitter Filtered Stream Api Client.
    /// </summary>
    public interface ITwitterFilteredStreamApiClient
    {
        /// <summary>
        /// Gets or sets Curre Stream Rules.
        /// </summary>
        public SearchStreamRules? CurrentStreamRules { get; set; }

        /// <summary>
        /// Filtered Stream Ended.
        /// </summary>
        public event EventHandler<StreamEndedEventArgs>? FilteredStream_Ended;

        /// <summary>
        /// Filtered Stream Started.
        /// </summary>
        public event EventHandler<StreamStartedEventArgs>? FilteredStream_Started;

        /// <summary>
        /// Filtered Stream Tweets Received.
        /// </summary>
        public event EventHandler<StreamFilteredTweetReceivedEventArgs>? FilteredStream_TweetsReceived;

        /// <summary>
        /// Get Tweet Stream Rules.
        /// </summary>
        /// <returns>Stream Rules.</returns>
        public Task<SearchStreamRules?> GetTweetStreamRules();

        /// <summary>
        /// Set Tweet Batching Millisonds. After milliseconds have elapsed, all feed items that have been accumlated will be included in <see cref="FilteredStream_TweetsReceived"/>.
        /// </summary>
        /// <param name="batchingMilliseconds">Milliseconds to wait to accumlate feed events before firing <see cref="FilteredStream_TweetsReceived"/>.</param>
        /// <remarks>Batching size may occur before batching milliseconds reached and prevent batching milliseconds from being reached.</remarks>
        public void SetTweetBatchingMilliseconds(int batchingMilliseconds);

        /// <summary>
        /// Set Tweet Batching Size. After feed items have equaled or exceeded batch size, all feed items that have been accumlated will be included in <see cref="FilteredStream_TweetsReceived"/>.
        /// </summary>
        /// <param name="batchingSize"></param>
        /// <remarks>Batching milliseconds may occur before batching size reached and prevent batching size from reaching size limit.</remarks>
        public void SetTweetBatchingSize(int batchingSize);

        /// <summary>
        /// Set Tweet Stream Rules in Twitter Api. Subsequent streams established will contain these rules.
        /// </summary>
        /// <param name="dryRun">True if stream should not actual return a stream but to validate rules are set correctly.</param>
        /// <param name="searchStreamRules">Search Stream rules to set. No stream rules being set will not filter tweet stream.</param>
        /// <returns>Search stream rules set response from Twitter Api.</returns>
        public Task<SearchStreamResponse?> SetTweetStreamRulesAsync(bool? dryRun = null, SearchStreamRules? searchStreamRules = null);

        /// <summary>
        /// Start Tweeter streamer. Listen to events for outputs from stream.
        /// </summary>
        /// <param name="searchStreamRules">Optional: Rules to use in place of rules already set in Twitter Api.</param>
        /// <param name="cancellationtoken">Optional: Cancellation Token.</param>
        public Task StartTweetStreamer(SearchStreamRules? searchStreamRules = null, CancellationToken? cancellationtoken = null);
    }
}