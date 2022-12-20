namespace Feed.Runner.Lib.Tests.Support
{
    using System;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;

    /// <summary>
    /// Logger for testing.
    /// </summary>
    /// <typeparam name="T">Class type of logger.</typeparam>
    public class TestLogger<T> : ILogger<T>
    {
        private readonly string categoryName;
        private readonly ITestOutputHelper outputHelper;
        private readonly string logPrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLogger{T}"/> class.
        /// </summary>
        /// <param name="categoryName">Category name.</param>
        /// <param name="outputHelper">Test output helper.</param>
        /// <param name="logPrefix">Log prefix.</param>
        public TestLogger(string categoryName, ITestOutputHelper outputHelper, string? logPrefix = null)
        {
            this.categoryName = categoryName;
            this.outputHelper = outputHelper;
            this.logPrefix = logPrefix ?? string.Empty;
        }

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            if (exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            string message = this.logPrefix;
            if (formatter != null)
            {
                message += formatter(state, exception);
            }

            var output = $"{logLevel} - {eventId.Id} - {this.categoryName} - {message}";
            this.outputHelper.WriteLine(output);
            Debug.WriteLine(output);
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
