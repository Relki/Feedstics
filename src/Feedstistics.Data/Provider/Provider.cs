namespace Feedstistics.Data.Provider
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Provider : IFeedstisticsDataProvider
    {
        /// <summary>
        /// Gets Settings.
        /// </summary>
        private FeedstisticsDataSettings Settings { get; }

        /// <summary>
        /// Instansiates an instance of <see cref="Provider"/>.
        /// </summary>
        public Provider(
            FeedstisticsDataSettings settings)
        {
            this.StatisticNames = this.GetStatisticNames();
            this.Settings = settings;
        }

        /// <summary>
        /// Gets or sets DisabledStatisticNames.
        /// </summary>
        protected string[] StatisticNames { get; set; }

        /// <inheritdoc/>
        public abstract Task<IStatistic[]> GetStatisticsAsync(string statisticName, DateTime startDate, DateTime? endDate = default!);

        /// <inheritdoc/>
        public abstract Task<IStatistic?> GetLatestStatisticAsync(string statisticName);

        /// <inheritdoc/>
        public abstract Task<IStatistic[]> GetLatestStatisticsAsync();

        /// <inheritdoc/>
        public abstract Task<IStatistic[]> GetStatisticsAsync(DateTime startDate, DateTime? endDate = default!);

        /// <inheritdoc/>
        public string[] GetStatisticNames()
        {
            var type = typeof(IStatistic);
            var statisticTypeNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(assemblyType =>
                    type.IsAssignableFrom(assemblyType) &&
                    !assemblyType.IsInterface &&
                    !assemblyType.IsAbstract &&
                    assemblyType.Name != "StatisticBase`1" &&
                    !this.Settings.DisabledStatisticNames.Any(disabledStatisticName => disabledStatisticName == assemblyType.Name))
                .Select(assemblyType => assemblyType.Name)
                .ToArray();

            return statisticTypeNames;
        }

        /// <inheritdoc/>
        public Type? GetStatisticType(string statisticName)
        {
            var type = typeof(IStatistic);
            var statisticTypeNames = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(assemblyType =>
                    type.IsAssignableFrom(assemblyType) &&
                    !assemblyType.IsInterface &&
                    !assemblyType.IsAbstract &&
                    assemblyType.Name.Equals(statisticName, StringComparison.InvariantCultureIgnoreCase));

            return statisticTypeNames?.GetType();
        }
    }
}
