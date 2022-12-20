namespace Feed.Runner.Lib.FeedProvider
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Feed provider factory.
    /// </summary>
    public class FeedProviderFactory : IFeedProviderFactory
    {
        /// <summary>
        /// Feed Provider Settings.
        /// </summary>
        private FeedProviderSettings? _settings { get; }

        /// <summary>
        /// Logger.
        /// </summary>
        private ILogger<FeedProviderFactory> _logger { get; }

        /// <summary>
        /// ServiceProvider.
        /// </summary>
        private IServiceProvider _serviceProvider { get; }

        /// <summary>
        /// Instansiates an instance of <see cref="FeedProviderFactory"/>.
        /// </summary>
        /// <param name="settings">Feed Provider Settings.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public FeedProviderFactory(
            IServiceProvider serviceProvider,
            IOptions<FeedProviderSettings> settings,
            ILoggerFactory loggerFactory)
        {
            this._serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this._settings = settings.Value ?? new FeedProviderSettings();
            this._logger = loggerFactory.CreateLogger<FeedProviderFactory>();
        }

        /// <inheritdoc/>
        public IFeedProvider? GetFeedProvider(string feedProviderName)
        {
            var feedProvider = GetReflectedFeedProviders()
                .Where(feedProvider => feedProvider.FeedProviderName.Equals(feedProviderName, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            return feedProvider;
        }

        /// <inheritdoc/>
        public IFeedProvider[] GetFeedProviders(bool? enabled = null)
        {
            var feedProviders = GetReflectedFeedProviders();

            if (this._settings == null || this._settings.EnabledFeedProviderNames == null)
            {
                return feedProviders.ToArray();
            }

            // Check settings to see what feed providers are enabled and only return those feed provider instances.
            if (enabled.HasValue && enabled.Value)
            {
                return feedProviders
                .Where(feedProvider => this._settings.EnabledFeedProviderNames.Any(enabledFeedProvider => enabledFeedProvider.Equals(feedProvider.FeedProviderName, StringComparison.InvariantCultureIgnoreCase)))
                .ToArray();
            }
            // Check settings to see what feed providers are enabled and only return disabled feed provider instances.
            else if (enabled.HasValue)
            {
                return feedProviders
                .Where(feedProvider => !this._settings.EnabledFeedProviderNames.All(enabledFeedProvider => enabledFeedProvider.Equals(feedProvider.FeedProviderName, StringComparison.InvariantCultureIgnoreCase)))
                .ToArray();
            }

            // Return all feed providers, regardless if enabled or disabed.
            return feedProviders.ToArray();
        }

        /// <summary>
        /// Reflect feed providers available in this assembly and instantiate and return them.
        /// </summary>
        /// <returns>Collection of reflected Feed Providers.</returns>
        private IFeedProvider[] GetReflectedFeedProviders()
        {
            var type = typeof(IFeedProvider);
            var feedProviderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(assemblyType => type.IsAssignableFrom(assemblyType) && !assemblyType.IsInterface && !assemblyType.IsAbstract)
                .ToArray();

            var feedProviders = new List<IFeedProvider>();
            foreach (var feedProviderType in feedProviderTypes)
            {
                var feedProvider = ActivatorUtilities.CreateInstance(this._serviceProvider, feedProviderType);
                if (feedProvider != null)
                {
                    feedProviders.Add((IFeedProvider)feedProvider);
                }
            }

            return feedProviders.ToArray();
        }
    }
}
