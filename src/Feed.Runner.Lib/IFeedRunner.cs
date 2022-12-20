namespace Feed.Runner.Lib
{
    using Feedstistics.Shared;

    /// <summary>
    /// Feed Runner interface.
    /// </summary>
    public interface IFeedRunner
    {
        /// <summary>
        /// Starts all enabled feeds.
        /// </summary>
        Task StartFeeds(CancellationToken? cancellationToken = default!);

        /// <summary>
        /// Stops all feeds.
        /// </summary>
        Task<bool> StopFeeds(CancellationToken? cancellationToken = default!);

        /// <summary>
        /// Export feed items.
        /// </summary>
        /// <param name="feedItems">Feed items to export.</param>
        public void ExportFeedItems(IEnumerable<IFeedstisticsMessage> feedItems);
    }
}