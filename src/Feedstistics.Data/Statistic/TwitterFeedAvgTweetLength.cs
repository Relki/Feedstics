namespace Feedstistics.Data.Statistic
{
    /// <summary>
    /// Twitter Feed Average Tweet Length.
    /// </summary>
    public class TwitterFeedAvgTweetLength : StatisticBase<decimal>
    {
        /// <summary>
        /// Insansiates an instance of <see cref="TwitterFeedAvgTweetLength"/>.
        /// </summary>
        public TwitterFeedAvgTweetLength()
            : base()
        {

        }

        /// <summary>
        /// Insansiates an instance of <see cref="TwitterFeedAvgTweetLength"/> with an Statistic entity.
        /// </summary>
        public TwitterFeedAvgTweetLength(EF.Models.Entity.Statistics statistic)
            : base(statistic)
        {

        }

        /// <summary>
        /// Insansiates an instance of <see cref="TwitterFeedAvgTweetLength"/> with a serialized dimension.
        /// </summary>
        public TwitterFeedAvgTweetLength(string serializedValueDimensions, DateTime sampleDateTimeUtc)
    :       base(nameof(TwitterFeedAvgTweetLength), serializedValueDimensions, sampleDateTimeUtc)
        {
        }
    }
}
