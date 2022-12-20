namespace Feed.Runner
{
    using Feed.Runner.Lib;

    /// <summary>
    /// Feed runner worker.
    /// </summary>
    public class Worker : BackgroundService
    {
        private readonly IFeedRunner _feedRunner;
        private readonly ILogger<Worker> _logger;

        /// <summary>
        /// Instantiates a new instance of <see cref="Worker"/>.
        /// </summary>
        /// <param name="feedRunner">Feed runner.</param>
        /// <param name="loggerFactory">Logger.</param>
        public Worker(
            IFeedRunner feedRunner,
            ILoggerFactory loggerFactory)
        {
            _feedRunner = feedRunner;
            _logger = loggerFactory.CreateLogger<Worker>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _feedRunner.StartFeeds(stoppingToken);
            }
        }
    }
}