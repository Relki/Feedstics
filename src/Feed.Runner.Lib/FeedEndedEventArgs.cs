namespace Feed.Runner.Lib
{
    /// <summary>
    /// Feed Ended Event Arguments.
    /// </summary>
    public class FeedEndedEventArgs : EventArgs
    {
        /// <summary>
        /// Instansiates an new instance of <see cref="FeedEndedEventArgs"/>.
        /// </summary>
        /// <param name="feedProviderName"></param>
        /// <param name="endedDateTimeUtc"></param>
        /// <param name="feedEndReason"></param>
        public FeedEndedEventArgs(string feedProviderName, DateTime? endedDateTimeUtc = null, string? feedEndReason = null)
        {
            this.EndedDateTimeUtc = endedDateTimeUtc ?? DateTime.UtcNow;
            this.FeedProviderName = feedProviderName;
            this.FeedEndReason = feedEndReason;
        }

        /// <summary>
        /// Gets or sets the Feed Provider Name.
        /// </summary>
        public string? FeedProviderName { get; set; }

        /// <summary>
        /// Gets or sets the Ended Date Time in UTC.
        /// </summary>
        public DateTime EndedDateTimeUtc { get; set; }

        /// <summary>
        /// Gets or sets the Feed End Reson.
        /// </summary>
        public string? FeedEndReason { get; set; }
    }
}