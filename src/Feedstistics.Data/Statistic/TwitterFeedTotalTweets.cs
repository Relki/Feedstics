namespace Feedstistics.Data.Statistic
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Twitter Feed Total Tweets.
    /// </summary>
    public class TwitterFeedTotalTweets : StatisticBase<int>
    {
        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedTotalTweets"/>.
        /// </summary>
        public TwitterFeedTotalTweets()
            : base()
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedTotalTweets"/> with an Statistic entity.
        /// </summary>
        public TwitterFeedTotalTweets(EF.Models.Entity.Statistics statistic)
            : base(statistic)
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedTotalTweets"/> with a serialized dimension.
        /// </summary>
        public TwitterFeedTotalTweets(string serializedValueDimensions, DateTime sampleDateTimeUtc)
            : base(nameof(TwitterFeedTotalTweets), serializedValueDimensions, sampleDateTimeUtc)
        {
        }
    }
}
