namespace Feedstistics.Data.Statistic
{
    /// <summary>
    /// Statistic factory.
    /// </summary>
    public sealed class StatisticFactory : IStatisticFactory
    {
        /// <inheritdoc/>
        public IStatistic GetStatistic(string statisticName, string serializedValueDimensions, DateTime sampleDateTimeUtc)
        {
            return statisticName switch
            {
                nameof(TwitterFeedAvgTweetLength) => new TwitterFeedAvgTweetLength(serializedValueDimensions, sampleDateTimeUtc),
                nameof(TwitterFeedTotalTweets) => new TwitterFeedTotalTweets(serializedValueDimensions, sampleDateTimeUtc),
                nameof(TwitterFeedAnnotationCount) => new TwitterFeedAnnotationCount(serializedValueDimensions, sampleDateTimeUtc),
                nameof(TwitterFeedHashtagCount) => new TwitterFeedHashtagCount(serializedValueDimensions, sampleDateTimeUtc),
                _ => new StatisticBase<object>(statisticName, serializedValueDimensions, sampleDateTimeUtc),
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
