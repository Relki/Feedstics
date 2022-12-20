namespace Feed.Runner.Lib.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Twitter.Api.Client;

    public class Startup
    {
        /// <summary>
        /// Configure test host.
        /// </summary>
        /// <param name="hostBuilder">Host builder.</param>
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                _ = config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            });

            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddOptions<TwitterClientSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(nameof(TwitterClientSettings)).Bind(settings);
                    });
            });
        }

        /// <summary>
        /// Configure dependency injection.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="context">context.</param>
        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            services.AddTransient<TwitterFilteredStreamApiClient, TwitterFilteredStreamApiClient>();
        }
    }
}
