namespace Feed.Runner.Lib.FeedProvider
{
    /// <summary>
    /// Feed Provider Factory interface.
    /// </summary>
    public interface IFeedProviderFactory
    {
        /// <summary>
        /// Get feed providers.
        /// </summary>
        /// <param name="enabled">Optional: True to return enabled feed providers, False to return disabled feed providers. If left null, all feed providers are returned.</param>
        /// <returns>Collection of Feed Providers.</returns>
        public IFeedProvider[] GetFeedProviders(bool? enabled = null);

        /// <summary>
        /// Get a specific feed provider by name.
        /// </summary>
        /// <param name="feedProviderName">Feed Provider name.</param>
        /// <returns>Feed Provider.</returns>
        public IFeedProvider? GetFeedProvider(string feedProviderName);
    }
}
