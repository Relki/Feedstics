namespace Feed.Runner.Lib.Tests.Support
{
    using Microsoft.Extensions.Logging;
    using Xunit.Abstractions;

    public class TwitterRunnerLibTestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterRunnerLibTestBase"/> class.
        /// </summary>
        /// <param name="output">Test output helper for logging.</param>
        public TwitterRunnerLibTestBase(ITestOutputHelper output)
        {
            this.Output = output;
            this._Logger = new TestLogger<TwitterRunnerLibTestBase>(nameof(TwitterRunnerLibTestBase), this.Output);
        }

        /// <summary>Gets or sets the test log output class.</summary>
        /// <value>The output.</value>
        protected ILogger _Logger { get; set; }

        /// <summary>Gets or sets the test log output class.</summary>
        /// <value>The output.</value>
        protected ITestOutputHelper Output { get; set; }
    }
}
