namespace Feedstistics.Lib
{
    using Feedstistics.Data;

    /// <summary>
    /// Feed statistics Service.
    /// </summary>
    public interface IFeedstisticsService
    {
        /// <summary>
        /// Get available statistic names.
        /// </summary>
        /// <returns>Collection of statistic names.</returns>
        public string[] GetStatisticNames();

        /// <summary>
        /// Get latest statistic.
        /// </summary>
        /// <param name="statisticName"></param>
        /// <returns>Latest statistic.</returns>
        public Task<IStatistic?> GetLatestStatisticAsync(string statisticName);

        /// <summary>
        /// Get all latest statistics available.
        /// </summary>
        /// <returns>Collection of latest statistics.</returns>
        public Task<IStatistic[]> GetLatestStatisticsAsync();

        /// <summary>
        /// Get all statistics within a date range.
        /// </summary>
        /// <param name="rangeStart">Starting point to get statistics from.</param>
        /// <param name="rangeEnd">Optional: Ending point to get statistics to.</param>
        /// <returns></returns>
        public Task<IStatistic[]> GetStatisticsAsync(DateTime rangeStart, DateTime? rangeEnd = default!);

        /// <summary>
        /// Get all statistics for a given statistic name within a date range.
        /// </summary>
        /// <param name="statisticName">Statistic name.</param>
        /// <param name="rangeStart">Starting point to get statistics from.</param>
        /// <param name="rangeEnd">Optional: Ending point to get statistics to.</param>
        /// <returns></returns>
        public Task<IStatistic[]> GetStatisticsAsync(string statisticName, DateTime rangeStart, DateTime? rangeEnd = default!);
    }
}