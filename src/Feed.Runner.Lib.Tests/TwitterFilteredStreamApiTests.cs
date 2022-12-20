namespace Feed.Runner.Lib.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Twitter.Api.Client;
    using Feed.Runner.Lib.Tests.Support;
    using Xunit.Abstractions;

    public class TwitterFilteredStreamApiTests
        : TwitterRunnerLibTestBase//, IClassFixture<IOptions<TwitterClientSettings>>, IClassFixture<TwitterFilteredStreamApiClient>
    {
        //private TwitterClientSettings? _twitterClientTestSettings;
        private readonly ILoggerFactory? _loggerFactory;
        private readonly TwitterFilteredStreamApiClient? _streamClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterFilteredStreamApiTests"/> class.
        /// </summary>
        /// <param name="output">Test output.</param>
        public TwitterFilteredStreamApiTests(
            ITestOutputHelper output,
            ILoggerFactory loggerFactory,
            TwitterFilteredStreamApiClient streamClient)
            : base(output)
        {
            //this._twitterClientTestSettings = twitterClientTestSettings.val;
            this._loggerFactory = loggerFactory;
            this._streamClient = streamClient;
        }

        [Fact]
        public async Task TwitterApiClientGetRules_HasRules()
        {
            //using var serviceProvider = this._services.BuildServiceProvider();
            //var twitterClient = new TwitterFilteredStreamApiClient(this._loggerFactory, this._twitterClientTestSettings);
            //var rulesResponse = await _streamClient.SetTweetStreamRulesAsync();

            
            await _streamClient.StartTweetStreamer();
        }
    }
}