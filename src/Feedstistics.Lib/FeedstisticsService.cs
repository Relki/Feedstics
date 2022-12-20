namespace Feedstistics.Lib
{
    using Feedstistics.Data;
    using Feedstistics.Data.Statistic;
    using Microsoft.Extensions.Logging;
    using System;

    /// <summary>
    /// Feedstistics Service managing statistics data.
    /// </summary>
    public sealed class FeedstisticsService : IFeedstisticsService
    {
        public FeedstisticsService(
            IFeedstisticsDataProvider feedDataProvider,
            ILoggerFactory loggerFactory)
        {
            this.FeedDataProvider = feedDataProvider;
            this.Logger = loggerFactory.CreateLogger<FeedstisticsService>();
        }


        /// <summary>
        /// Gets the feed data provider.
        /// </summary>
        private IFeedstisticsDataProvider FeedDataProvider { get; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private ILogger<FeedstisticsService> Logger { get; }

        /// <inheritdoc/>
        public async Task<IStatistic?> GetLatestStatisticAsync(string statisticName)
        {
            try
            {
                var statistic = await this.FeedDataProvider.GetLatestStatisticAsync(statisticName);
                return statistic;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception occured while retreiving latest statistic: [{statisticName}]");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IStatistic[]> GetLatestStatisticsAsync()
        {
            try
            {
                var statistics = await this.FeedDataProvider.GetLatestStatisticsAsync();
                return statistics;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception occured while retreiving all latest.");
                throw;
            }
        }

        /// <inheritdoc/>
        public string[] GetStatisticNames()
        {
            try
            {
                var statisticNames = this.FeedDataProvider.GetStatisticNames();
                return statisticNames;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Exception occured while retreiving statistic names.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IStatistic[]> GetStatisticsAsync(DateTime rangeStart, DateTime? rangeEnd = default!)
        {
            try
            {
                var statistics = await this.FeedDataProvider.GetStatisticsAsync(rangeStart, rangeEnd);
                return statistics;
            }
            catch (Exception ex)
            {
                var rangeEndString = rangeEnd.HasValue ? $", to [{rangeEnd}]" : string.Empty;
                this.Logger.LogError(ex, $"Exception occured while retreiving all statistics from: [{rangeStart}]{rangeEndString}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IStatistic[]> GetStatisticsAsync(string statisticName, DateTime rangeStart, DateTime? rangeEnd = null)
        {
            try
            {
                IStatistic[] statistics;
                switch (statisticName.ToLower())
                {
                    case "twitterfeedhashtagtop10count":
                        statistics = await GetTwitterFeedTop10HashtagCount(rangeStart, rangeEnd);
                        return statistics;
                    default:
                        statistics = await this.FeedDataProvider.GetStatisticsAsync(statisticName, rangeStart, rangeEnd);
                        return statistics;
                }
            }
            catch (Exception ex)
            {
                var rangeEndString = rangeEnd.HasValue ? $", to [{rangeEnd}]" : string.Empty;
                this.Logger.LogError(ex, $"Exception occured while retreiving all statistics for statistic name: [{statisticName}], from: [{rangeStart}]{rangeEndString}");
                throw;
            }
        }

        private async Task<TwitterFeedHashtagTop10Count[]> GetTwitterFeedTop10HashtagCount(DateTime rangeStart, DateTime? rangeEnd = null)
        {
            var statistics = (await this.FeedDataProvider.GetStatisticsAsync(nameof(TwitterFeedHashtagCount), rangeStart, rangeEnd))
                .Cast<TwitterFeedHashtagCount>();

            var top10Hashtags = statistics
                .GroupBy(statistic => new { statistic.Hashtag }, (key, hashtagCounts) =>
                    new TwitterFeedHashtagTop10Count()
                    {
                        Dimensions = new Dictionary<string, dynamic>
                        {
                            { "Hashtag", key.Hashtag ?? string.Empty },
                            { "Value", hashtagCounts.Sum(hashtagCounts => hashtagCounts.Value)},
                        },
                        SampleDateTime = hashtagCounts.Max(hashtagCounts => hashtagCounts.SampleDateTime)
                    })
               .OrderByDescending(hashtagCounts => hashtagCounts.Value)
               .Take(10)
               .ToArray();

            return top10Hashtags;
        }
    }
}
