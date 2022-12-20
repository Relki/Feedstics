namespace Feedstistics.Data.Statistic
{
    /// <summary>
    /// Statistic factory.
    /// </summary>
    public sealed class StatisticFactory : IStatisticFactory
    {
        /// <inheritdoc/>
        public IStatistic GetStatistic(string statisticName, string serializedValueDimensions, DateTime sampleDateTime)
        {
            return statisticName switch
            {
                nameof(TwitterFeedAvgTweetLength) => new TwitterFeedAvgTweetLength(serializedValueDimensions),
                nameof(TwitterFeedTotalTweets) => new TwitterFeedTotalTweets(serializedValueDimensions),
                nameof(TwitterFeedAnnotationCount) => new TwitterFeedAnnotationCount(serializedValueDimensions),
                nameof(TwitterFeedHashtagCount) => new TwitterFeedHashtagCount(serializedValueDimensions),
                _ => new StatisticBase<object>(statisticName, serializedValueDimensions),
            };
        }

        /// <inheritdoc/>
        public IStatistic GetStatistic(EF.Models.Entity.Statistics statistic)
        {
            return statistic.StatisticName switch
            {
                nameof(TwitterFeedAvgTweetLength) => new TwitterFeedAvgTweetLength(statistic),
                nameof(TwitterFeedTotalTweets) => new TwitterFeedTotalTweets(statistic),
                nameof(TwitterFeedAnnotationCount) => new TwitterFeedAnnotationCount(statistic),
                nameof(TwitterFeedHashtagCount) => new TwitterFeedHashtagCount(statistic),
                _ => new StatisticBase<object>(statistic),
            };
        }

        /// <inheritdoc/>
        public IEnumerable<IStatistic> GetStatistics(IEnumerable<EF.Models.Entity.Statistics> statistics)
        {
            return statistics.Select(this.GetStatistic);
        }
    }
}
