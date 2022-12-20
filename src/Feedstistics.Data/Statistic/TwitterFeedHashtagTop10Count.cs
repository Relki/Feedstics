namespace Feedstistics.Data.Statistic
{
    /// <summary>
    /// Twitter Feed Hashtag Top 10 Count.
    /// </summary>
    public class TwitterFeedHashtagTop10Count : TwitterFeedHashtagCount
    {
        /// <summary>
        /// Insansiates an instance of <see cref="TwitterFeedHashtagTop10Count"/>.
        /// </summary>
        public TwitterFeedHashtagTop10Count()
            : base()
        {
            this.Name = nameof(TwitterFeedHashtagTop10Count);
        }

        /// <summary>
        /// Insansiates an instance of <see cref="TwitterFeedHashtagTop10Count"/> with an existing statistic base fields.
        /// </summary>
        public TwitterFeedHashtagTop10Count(IStatistic statistic)
            : base(statistic)
        {
            this.Name = nameof(TwitterFeedHashtagTop10Count);
        }
    }
}
