namespace Feedstistics.Data
{
    /// <summary>
    /// Statistics factory returning unboxed typed statistics data provider.
    /// </summary>
    public interface IStatisticFactory
    {
        /// <summary>
        /// Get statistic given a statistic name and serialized value dimensions.
        /// </summary>
        /// <param name="statisticName">Name of statistic.</param>
        /// <param name="serializedValueDimensions">Serialized value dimensions.</param>
        /// <param name="sampleDateTime">Sample date time.</param>
        /// <returns>Unboxed Typed Statistic.</returns>
        IStatistic GetStatistic(string statisticName, string serializedValueDimensions, DateTime sampleDateTime);

        /// <summary>
        /// Gets statistic from an entity framework model Statistic row.
        /// </summary>
        /// <param name="statistic">Entity framework statistic.</param>
        /// <returns>Unboxed Typed Statistic.</returns>
        IStatistic GetStatistic(EF.Models.Entity.Statistics statistic);

        /// <summary>
        /// Get collection of statistics from a collection of entity framework model Statistic rows.
        /// </summary>
        /// <param name="statistic"></param>
        /// <returns></returns>
        IEnumerable<IStatistic> GetStatistics(IEnumerable<EF.Models.Entity.Statistics> statistic);
    }
}
