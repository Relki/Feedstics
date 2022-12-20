namespace Feed.Runner.Lib.FeedProvider
{
    /// <summary>
    /// Feed Provider settings.
    /// </summary>
    public class FeedProviderSettings
    {
        /// <summary>
        /// Gets or sets Enabled Feed Provider names.
        /// </summary>
        public string[]? EnabledFeedProviderNames { get; set; } = new[] { "Twitter" };
    }
}
