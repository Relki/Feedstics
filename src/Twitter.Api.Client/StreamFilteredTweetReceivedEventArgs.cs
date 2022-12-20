namespace Twitter.Api.Client
{
    using Twitter.Api.Data;

    /// <summary>
    /// Stream filtered tweet received event args.
    /// </summary>
    public class StreamFilteredTweetReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// DateTime this batch of tweets were batched.
        /// </summary>
        public DateTime BatchedDateTimeUtc { get; set; }

        /// <summary>
        /// Batch of tweets.
        /// </summary>
        public Tweet[] TweetBatch;

        /// <summary>
        /// Initializes an instance of <see cref="StreamFilteredTweetReceivedEventArgs"/> and sets Batched time UTC.
        /// </summary>
        /// <param name="tweets">Tweets received in this batch.</param>
        public StreamFilteredTweetReceivedEventArgs(Tweet[] tweets)
        {
            this.BatchedDateTimeUtc = DateTime.UtcNow;
            this.TweetBatch = tweets;
        }
    }

}