namespace Feed.Runner.Lib.FeedExporter
{
    using Feedstistics.Shared;

    /// <summary>
    /// Feed Exporter.
    /// </summary>
    public interface IFeedExporter
    {
        /// <summary>
        /// Gets or sets the name of this Feed Exporter.
        /// </summary>
        public string FeedExporterName { get; set; }

        /// <summary>
        /// Export feed items.
        /// </summary>
        /// <param name="feedItems">Feed items to export.</param>
        /// <param name="cancellationToken">Optional: CancellationToken.</param>
        public Task ExportFeed(IEnumerable<IFeedstisticsMessage> feedItems, CancellationToken cancellationToken = default);
    }
}
