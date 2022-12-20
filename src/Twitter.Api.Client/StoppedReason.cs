namespace Twitter.Api.Client
{
    /// <summary>
    /// Reason a search stream is stopped.
    /// </summary>
    public enum StoppedReason
    {
        /// <summary>
        /// No reason.
        /// </summary>
        None,

        /// <summary>
        /// Unexpected exception occurred.
        /// </summary>
        Exception,
    }
}