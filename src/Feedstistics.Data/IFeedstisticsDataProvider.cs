namespace Feedstistics.Data
{
    /// <summary>
    /// Feed statistics data provider.
    /// </summary>
    public interface IFeedstisticsDataProvider
    {
        public string[] GetStatisticNames();

        /// <summary>
        /// Get latest statistic data for a specific statistic name.
        /// </summary>
        /// <param name="statisticName"></param>
        /// <returns></returns>
        public Task<IStatistic?> GetLatestStatisticAsync(string statisticName);

        /// <summary>
        /// Get statistic data within range for a specific statistic name.
        /// </summary>
        /// <param name="statisticName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Task<IStatistic[]> GetStatisticsAsync(string statisticName, DateTime startDate, DateTime? endDate = default!);

        /// <summary>
        /// Get all latest statistics.
        /// </summary>
        /// <returns></returns>
        public Task<IStatistic[]> GetLatestStatisticsAsync();

        /// <summary>
        /// Get all statistic data within a range.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Task<IStatistic[]> GetStatisticsAsync(DateTime startDate, DateTime? endDate = default!);

        /// <summary>
        /// Get statistic type from statistic name.
        /// </summary>
        /// <param name="statisticName"></param>
        /// <returns></returns>
        public Type? GetStatisticType(string statisticName);
    }
}