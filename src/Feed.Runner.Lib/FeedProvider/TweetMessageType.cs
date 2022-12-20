namespace Feed.Runner.Lib.FeedProvider
{
    using Feedstistics.Shared;
    using Twitter.Api.Data;

    /// <summary>
    /// Tweet Message Type.
    /// </summary>
    public class TweetMessageType : Tweet, IFeedstisticsMessage
    {
        /// <summary>
        /// Gets or sets the Type of message.
        /// </summary>
        public string MessageType { get; set; } = "Tweet";

        /// <summary>
        /// Instansiates a new instance of <see cref="TweetMessageType"/>.
        /// </summary>
        public TweetMessageType() { }

        /// <summary>
        /// Instansiates a new instance of <see cref="TweetMessageType"/> from a <see cref="Tweet"/>.
        /// </summary>
        public TweetMessageType(Tweet tweet) 
        {
            this.Data = tweet.Data;
            this.Includes = tweet.Includes;
            this.MatchingRules = tweet.MatchingRules;
        }
    }
}
