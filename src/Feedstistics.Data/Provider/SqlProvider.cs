namespace Feedstistics.Data.Provider
{
    using Feedstistics.EF.Models.Context;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// SQL Provider for Statistics data.
    /// </summary>
    public sealed class SqlProvider : Provider
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private ILogger<SqlProvider> Logger { get; }

        /// <summary>
        /// Statistic Factory.
        /// </summary>
        public IStatisticFactory StatisticFactory { get; }

        /// <summary>
        /// Feedstistics database context.
        /// </summary>
        private FeedstisticsModelContext Context { get; }

        /// <summary>
        /// Instansiates an instance of <see cref="SqlProvider"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="statisticFactory">Statistic Factory.</param>
        /// <param name="context">Sql database context.</param>
        public SqlProvider(
            ILogger<SqlProvider> logger,
            IStatisticFactory statisticFactory,
            FeedstisticsModelContext context)
            : base()
        {
            this.Logger = logger;
            this.StatisticFactory = statisticFactory;
            this.Context = context;
        }

        /// <inheritdoc/>
        public override async Task<IStatistic?> GetLatestStatisticAsync(string statisticName)
        {
            var latestStatistic = await this.Context.Statistics.AsNoTracking()
                .Where(statistic => statistic.StatisticName == statisticName)
                .OrderByDescending(statistic => statistic.SampleDateTime)
                .FirstOrDefaultAsync();

            if (latestStatistic == null)
            {
                return null;
            }

            return this.StatisticFactory.GetStatistic(latestStatistic);
        }

        /// <inheritdoc/>
        public override async Task<IStatistic[]> GetLatestStatisticsAsync()
        {
            var latestStatisticsSampleDateTime = await this.Context.Statistics.AsNoTracking()
                .GroupBy(statistic => statistic.StatisticName,
                    (statisticName, statistics) =>
                    new
                    {
                        StatisticName = statisticName,
                        MaxStatisticSampleTime = statistics.Max(statistic => statistic.SampleDateTime)
                    })
                .Join(
                    this.Context.Statistics.AsNoTracking(),
                    innerCollection => innerCollection,
                    outerCollection => new { outerCollection.StatisticName, MaxStatisticSampleTime = outerCollection.SampleDateTime},
                    (inner, outer) => outer)
                .Select(latestStatistic => latestStatistic)
                .ToListAsync();

            return this.StatisticFactory.GetStatistics(latestStatisticsSampleDateTime).ToArray();
        }

        /// <inheritdoc/>
        public override async Task<IStatistic[]> GetStatisticsAsync(string statisticName, DateTime startDate, DateTime? endDate = null)
        {
            var statistics = await this.Context.Statistics.AsNoTracking()
                .Where(statistic =>
                    // SQL is not case senstive by default for value comparisons
                    statistic.StatisticName == statisticName &&
                    statistic.SampleDateTime >= startDate &&
                    (endDate == null || statistic.SampleDateTime <= endDate))
                .ToListAsync();

            return this.StatisticFactory.GetStatistics(statistics).ToArray();
        }

        /// <inheritdoc/>
        public override async Task<IStatistic[]> GetStatisticsAsync(DateTime startDate, DateTime? endDate = null)
        {
            var statistics = await this.Context.Statistics.AsNoTracking()
                .Where(statistic =>
                    statistic.SampleDateTime >= startDate &&
                    (endDate == null || statistic.SampleDateTime <= endDate))
                .ToListAsync();

            return this.StatisticFactory.GetStatistics(statistics).ToArray();
        }
    }
}
