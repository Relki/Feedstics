namespace Feed.Runner.Lib
{
    using Feedstistics.Shared;

    /// <summary>
    /// Feed Updated Event Arguments.
    /// </summary>
    public class FeedUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// Instansiates a new instance of <see cref="FeedUpdateEventArgs"/>.
        /// </summary>
        /// <param name="feedSourceName">Feed Source Name.</param>
        /// <param name="updatedDateTimeUtc">Updated Date Time</param>
        /// <param name="feedItems">Feed items in this update.</param>
        public FeedUpdateEventArgs(string feedSourceName, DateTime? updatedDateTimeUtc = null, IEnumerable<IFeedstisticsMessage>? feedItems = null)
        {
            this.FeedSourceName = feedSourceName;
            this.DateTimeUpdatedUtc = updatedDateTimeUtc ?? DateTime.UtcNow;
            this.FeedItems = feedItems == null ? new List<IFeedstisticsMessage>() : feedItems.ToList();
        }

        /// <summary>
        /// Gets or sets the Feed Source Name.
        /// </summary>
        public string? FeedSourceName { get; set; }

        /// <summary>
        /// Gets or sets the DateTime of this update in UTC.
        /// </summary>
        public DateTime DateTimeUpdatedUtc { get; set; }

        /// <summary>
        /// Gets or sets the collection of Feed Items in this update.
        /// </summary>
        public List<IFeedstisticsMessage> FeedItems { get; set; } = new List<IFeedstisticsMessage>();
    }
}