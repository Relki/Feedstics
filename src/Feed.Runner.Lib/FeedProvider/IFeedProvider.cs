namespace Feed.Runner.Lib.FeedProvider
{
    using Feedstistics.Shared;

    /// <summary>
    /// Feed provider interface.
    /// </summary>
    public interface IFeedProvider
    {
        /// <summary>
        /// Feed provider has started.
        /// </summary>
        public event EventHandler<FeedStartedEventArgs>? Feed_Started;

        /// <summary>
        /// Feed provider has stopped.
        /// </summary>
        public event EventHandler<FeedEndedEventArgs>? Feed_Ended;

        /// <summary>
        /// Feed provider has produced updated feed item(s).
        /// </summary>
        public event EventHandler<FeedUpdateEventArgs>? Feed_Update;

        /// <summary>
        /// Gets or sets the name of this feed provider.
        /// </summary>
        public string FeedProviderName { get; set; }

        /// <summary>
        /// Start feed provider.
        /// </summary>
        /// <param name="cancellationToken">Optional: CancellationToken.</param>
        /// <returns>Task.</returns>
        public Task Start(CancellationToken? cancellationToken = default!);

        /// <summary>
        /// Stop feed provider.
        /// </summary>
        /// <param name="cancellationToken">Optional: CancellationToken.</param>
        /// <returns>True if provider stopped successfully. False if not.</returns>
        public Task<bool> Stop(CancellationToken? cancellationToken = default!);

        /// <summary>
        /// Attempt to convert object to IFeedstisticsMessage.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IFeedstisticsMessage ToFeedStatisticMessage(object message);
    }
}