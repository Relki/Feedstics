namespace Feed.Runner.Lib.FeedExporter
{
    /// <summary>
    /// Feed Exporter Factory interface.
    /// </summary>
    public interface IFeedExporterFactory
    {
        /// <summary>
        /// Get feed exporters.
        /// </summary>
        /// <param name="enabled">Optional: True to return enabled feed exporters, False to return disabled feed exporters. If left null, all feed exporters are returned.</param>
        /// <returns>Collection of Feed Providers.</returns>
        public IFeedExporter[] GetFeedExporters(bool? enabled = null);

        /// <summary>
        /// Get a specific feed exporter by name.
        /// </summary>
        /// <param name="feedExporterName">Feed Provider name.</param>
        /// <returns>Feed Provider.</returns>
        public IFeedExporter? GetFeedExporter(string feedProviderName);
    }
}
