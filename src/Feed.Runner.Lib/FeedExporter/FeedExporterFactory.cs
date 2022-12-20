namespace Feed.Runner.Lib.FeedExporter
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Feed exporter factory.
    /// </summary>
    public class FeedExporterFactory : IFeedExporterFactory
    {
        /// <summary>
        /// Feed Exporter Settings.
        /// </summary>
        private FeedExporterSettings? _settings { get; }

        /// <summary>
        /// Logger.
        /// </summary>
        private ILogger<FeedExporterFactory> _logger { get; }

        /// <summary>
        /// ServiceProvider.
        /// </summary>
        private IServiceProvider _serviceProvider { get; }

        /// <summary>
        /// Instansiates an instance of <see cref="FeedExporterFactory"/>.
        /// </summary>
        /// <param name="settings">Feed Exporter Settings.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public FeedExporterFactory(
            IServiceProvider serviceProvider,
            IOptions<FeedExporterSettings> settings,
            ILoggerFactory loggerFactory)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._settings = settings.Value ?? new FeedExporterSettings();
            this._logger = loggerFactory.CreateLogger<FeedExporterFactory>();
        }

        /// <inheritdoc/>
        public IFeedExporter? GetFeedExporter(string feedExporterName)
        {
            var feedExporter = GetReflectedFeedExporters()
                .Where(feedExporter => feedExporter.FeedExporterName.Equals(feedExporterName, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            return feedExporter;
        }

        /// <inheritdoc/>
        public IFeedExporter[] GetFeedExporters(bool? enabled = null)
        {
            var feedExporters = GetReflectedFeedExporters();

            if (this._settings == null || this._settings.EnabledFeedExporterNames == null)
            {
                return feedExporters.ToArray();
            }

            // Check settings to see what feed Exporters are enabled and only return those feed Exporter instances.
            if (enabled.HasValue && enabled.Value)
            {
                return feedExporters
                .Where(feedExporter => this._settings.EnabledFeedExporterNames.Any(enabledFeedExporter => enabledFeedExporter.Equals(feedExporter.FeedExporterName, StringComparison.InvariantCultureIgnoreCase)))
                .ToArray();
            }
            // Check settings to see what feed Exporters are enabled and only return disabled feed Exporter instances.
            else if (enabled.HasValue)
            {
                return feedExporters
                .Where(feedExporter => !this._settings.EnabledFeedExporterNames.All(enabledFeedExporter => enabledFeedExporter.Equals(feedExporter.FeedExporterName, StringComparison.InvariantCultureIgnoreCase)))
                .ToArray();
            }

            // Return all feed Exporters, regardless if enabled or disabed.
            return feedExporters.ToArray();
        }

        /// <summary>
        /// Reflect feed Exporters available in this assembly and instantiate and return them.
        /// </summary>
        /// <returns>Collection of reflected Feed Exporters.</returns>
        private IFeedExporter[] GetReflectedFeedExporters()
        {
            var type = typeof(IFeedExporter);
            var feedExporterTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(assemblyType => type.IsAssignableFrom(assemblyType) && !assemblyType.IsInterface && !assemblyType.IsAbstract)
                .ToArray();

            var feedExporters = new List<IFeedExporter>();
            foreach (var feedExporterType in feedExporterTypes)
            {
                
                var feedExporter = ActivatorUtilities.CreateInstance(this._serviceProvider, feedExporterType);
                if (feedExporter != null)
                {
                    feedExporters.Add((IFeedExporter)feedExporter);
                }
            }

            return feedExporters.ToArray();
        }
    }
}
