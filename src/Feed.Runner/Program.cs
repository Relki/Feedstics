namespace Feed.Runner
{
    using Feed.Runner.Lib;
    using Feed.Runner.Lib.FeedExporter;
    using Feed.Runner.Lib.FeedProvider;
    using Feedstistics.Shared;
    using Feedstistics.Shared.Event;
    using Feedstistics.Shared.Queue;
    using Twitter.Api.Client;

    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                services.AddHostedService<Worker>();
                services.AddOptions<TwitterClientSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(TwitterClientSettings)).Bind(settings);
                    });
                services.AddOptions<FeedProviderSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(FeedProviderSettings)).Bind(settings);
                    });
                services.AddOptions<FeedExporterSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(FeedExporterSettings)).Bind(settings);
                    });
                services.AddOptions<AzureStorageQueueClientSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(AzureStorageQueueClientSettings)).Bind(settings);
                    });
                services.AddOptions<AzureEventHubProducerClientSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(AzureEventHubProducerClientSettings)).Bind(settings);
                    });

                services.AddTransient<IQueueClient, AzureStorageQueueClient>();
                services.AddSingleton<IEventProducerClient<IFeedstisticsMessage>, AzureEventHubProducerClient<IFeedstisticsMessage>>();
                services.AddTransient<TwitterFilteredStreamApiClient, TwitterFilteredStreamApiClient>();
                services.AddTransient<IFeedProviderFactory, FeedProviderFactory>();
                services.AddTransient<IFeedExporterFactory, FeedExporterFactory>();

                    // Main service to run.
                    services.AddTransient<IFeedRunner, FeedRunner>();
                })
                .Build();

            host.Run();
        }
    }
}