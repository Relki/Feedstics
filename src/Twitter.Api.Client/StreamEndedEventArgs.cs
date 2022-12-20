namespace Twitter.Api.Client
{
    /// <summary>
    /// Stream ended event args.
    /// </summary>
    public class StreamEndedEventArgs : EventArgs
    {
        /// <summary>
        /// Reason stream was stopped.
        /// </summary>
        public StoppedReason Reason;

        /// <summary>
        /// Stream stopped date time UTC.
        /// </summary>
        public DateTime StoppedTimeUtc { get; set; }

        /// <summary>
        /// Intializes an instance of <see cref="StreamEndedEventArgs"/> with and sets stop time UTC.
        /// </summary>
        /// <param name="reason">Reason stream was stopped.</param>
        public StreamEndedEventArgs(StoppedReason reason)
        {
            this.Reason = reason;
            this.StoppedTimeUtc = DateTime.UtcNow;
        }
    }
}