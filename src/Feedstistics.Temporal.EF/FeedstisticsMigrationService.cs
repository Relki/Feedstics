namespace Feedstistics.EF
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Shared.ConsoleServices;
    using Feedstistics.EF.Models.Context;

    /// <summary>
    /// Feedstistics Model Migration Service.
    /// </summary>
    public class FeedstisticsMigrationService : CoordinatedBackgroundService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedstisticsMigrationService"/> class.
        /// </summary>
        /// <param name="context">Feedstistics database context.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="applicationLifetime">Application lifetime.</param>
        public FeedstisticsMigrationService(
            FeedstisticsModelContext context,
            ILogger<FeedstisticsMigrationService> logger,
            IHostApplicationLifetime applicationLifetime)
                : base(applicationLifetime)
        {
            this.Logger = logger;
            this.Context = context;
        }

        /// <summary>
        /// Gets logger.
        /// </summary>
        internal ILogger<FeedstisticsMigrationService> Logger { get; }

        /// <summary>
        /// Gets initial source context.
        /// </summary>
        internal FeedstisticsModelContext Context { get; }

        /// <inheritdoc/>
        protected override Task ExecuteAsync(CancellationToken appStoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
