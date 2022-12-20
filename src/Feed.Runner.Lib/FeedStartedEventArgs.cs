namespace Feed.Runner.Lib
{
    /// <summary>
    /// Feed Started Event Arguments.
    /// </summary>
    public class FeedStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Instansiates a new instance of <see cref="FeedStartedEventArgs"/>.
        /// </summary>
        /// <param name="feedProviderName">Feed Provider Name.</param>
        /// <param name="startedDateTimeUtc">Started DateTime in UTC></param>
        public FeedStartedEventArgs(string feedProviderName, DateTime? startedDateTimeUtc = null)
        {
            this.StartedDateTimeUtc = startedDateTimeUtc ?? DateTime.UtcNow;
            this.FeedProviderName = feedProviderName;
        }

        /// <summary>
        /// Gets or sets the Feed Provider Name.
        /// </summary>
        public string? FeedProviderName { get; set; }

        /// <summary>
        /// Gets or sets the Started DateTime in UTC>
        /// </summary>
        public DateTime StartedDateTimeUtc { get; set; }
    }
}