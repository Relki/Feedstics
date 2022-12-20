namespace Feed.Runner.Lib.FeedExporter
{
    /// <summary>
    /// Feed Exporeter Settings.
    /// </summary>
    public class FeedExporterSettings
    {
        /// <summary>
        /// Gets or sets a value indicationg if this feed exporter should export feed items individually.
        /// True, indicates each feed item will be exported individually. False indicates feed items will
        /// be sent as they are received in bulk (unaltered).
        /// </summary>
        public bool RequireIndividualItemExport { get; set; }

        /// <summary>
        /// Gets or sets Enabled Feed Exporter names.
        /// </summary>
        public string[]? EnabledFeedExporterNames { get; set; } // = new[] { "AzureQueue" };
    }
}
