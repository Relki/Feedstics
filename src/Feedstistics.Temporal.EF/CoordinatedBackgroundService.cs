namespace Shared.ConsoleServices
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Coordinated background service.
    /// </summary>
    public abstract class CoordinatedBackgroundService : IHostedService, IDisposable
    {
        private readonly CancellationTokenSource appStoppingTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinatedBackgroundService"/> class.
        /// </summary>
        /// <param name="applicationLifetime">Application lifetime.</param>
        protected CoordinatedBackgroundService(IHostApplicationLifetime applicationLifetime)
        {
            this.ApplicationLifetime = applicationLifetime;
        }

        /// <summary>
        /// Gets application lifetime.
        /// </summary>
        protected IHostApplicationLifetime ApplicationLifetime { get; }

        /// <summary>
        /// Start method.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Await-able task.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.ApplicationLifetime.ApplicationStarted.Register(
                async () =>
                {
                    await this.ExecuteAsync(this.appStoppingTokenSource.Token).ConfigureAwait(false);
                });
            return this.InitializingAsync(cancellationToken);
        }

        /// <summary>
        /// Stop method.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Await-able task.</returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.appStoppingTokenSource.Cancel();
            await this.StoppingAsync(cancellationToken).ConfigureAwait(false);
            this.Dispose();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public virtual void Dispose()
        {
            // NO-OP.
        }

        /// <summary>
        /// Initialize method.
        /// </summary>
        /// <param name="cancelInitToken">Cancellation token.</param>
        /// <returns>Await-able task.</returns>
        protected virtual Task InitializingAsync(CancellationToken cancelInitToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Execute method.
        /// </summary>
        /// <param name="appStoppingToken">Application stop token.</param>
        /// <returns>Await-able task.</returns>
        protected abstract Task ExecuteAsync(CancellationToken appStoppingToken);

        /// <summary>
        /// Stopping method.
        /// </summary>
        /// <param name="cancelStopToken">Cancellation token.</param>
        /// <returns>Await-able task.</returns>
        protected virtual Task StoppingAsync(CancellationToken cancelStopToken)
        {
            return Task.CompletedTask;
        }
    }
}
