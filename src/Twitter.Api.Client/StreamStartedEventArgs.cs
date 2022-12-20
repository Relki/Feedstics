namespace Twitter.Api.Client
{
    /// <summary>
    /// Stream started event args.
    /// </summary>
    public class StreamStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets Stream Started date time UTC.
        /// </summary>
        public DateTime StreamStartedStartTimeUtc { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="StreamStartedEventArgs"/> and sets start time.
        /// </summary>
        public StreamStartedEventArgs()
        {
            StreamStartedStartTimeUtc = DateTime.UtcNow;
        }
    }
}