namespace Feedstistics.Api
{
    using System;
    using Feedstistics.Data;
    using Feedstistics.Data.Provider;
    using Feedstistics.EF.Models.Context;
    using Feedstistics.Lib;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Feedstistics.Data.Statistic;

    /// <summary>
    /// Configure services.
    /// </summary>
    public static partial class ConfigureServices
    {
        /// <summary>
        /// Configure services.
        /// </summary>
        /// <param name="context">Host builder context.</param>
        /// <param name="services">Services collection.</param>
        internal static void ConfigureFunctionServices(HostBuilderContext context, IServiceCollection services)
        {
            try
            {
                services.AddOptions<FeedstisticsDataSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration
                            .GetSection("FeedstisticsDataSettings")
                            .Bind(settings);
                    });

                services
                    .AddApplicationInsightsTelemetryWorkerService()
                    .AddTransient<IFeedstisticsService, FeedstisticsService>()
                    .AddTransient<IFeedstisticsDataProvider, SqlProvider>()
                    .AddTransient<IStatisticFactory, StatisticFactory>()
                    .AddDbContext<FeedstisticsModelContext>((serviceProvider, options) =>
                    {
                        var config = serviceProvider.GetRequiredService<IConfiguration>();

                        var connectionString = config.GetConnectionString(nameof(FeedstisticsModelContext)) ?? throw new InvalidOperationException("FeedstisticsModelContext connection string missing from config. Please set FeedstisticsModelContext connection string value to the sql database connection string for Feedstistics database.");

                        if (string.IsNullOrWhiteSpace(connectionString))
                        {
                            throw new InvalidOperationException("FeedstisticsModelContext connection string is empty or is white spaces. Please set FeedstisticsModelContext connection string value to the sql database connection string for Feedstistics database.");
                        }

                        options.UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure());
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Startup configuration exception: {ex}");
                throw;
            }
        }
    }
}
