namespace Feedstistics.EF
{
    using System;
    using Feedstistics.EF.Models.Context;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Entry for Feedstistics Model Migration service.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args)
                    .UseConsoleLifetime()
                    .Build();

                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Main exception {ex.Message}");
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    ConfigureServices(services);
                });
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var configuration = new ConfigurationBuilder()
                .AddConfiguration(serviceProvider.GetRequiredService<IConfiguration>())
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.Development.json", true)
                .Build();

            services.AddDbContext<FeedstisticsModelContext>(
                (options) =>
                {
                    AddDbContext(
                        options,
                        nameof(FeedstisticsModelContext),
                        configuration);
                },
                ServiceLifetime.Singleton);

            services.AddHostedService<FeedstisticsMigrationService>();

            return services;
        }

        private static void AddDbContext(
            DbContextOptionsBuilder options,
            string settingKey,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(settingKey)
                ??
                configuration.GetValue<string>(settingKey);

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine($"Missing ConnectionStrings:{settingKey}.");
                throw new ArgumentException($"Missing ConnectionStrings:{settingKey}.");
            }

            var executionTimeoutMin = Math.Max(5, configuration.GetValue<int>("ExecutionTimeOutMinutes"));

            if (executionTimeoutMin == 0)
            {
                executionTimeoutMin = 1;
            }

            options.UseSqlServer(
                connectionString,
                providerOptions =>
                {
                    providerOptions.EnableRetryOnFailure();
                    providerOptions.CommandTimeout((int)TimeSpan.FromMinutes(executionTimeoutMin).TotalSeconds);
                    providerOptions.MigrationsAssembly("Feedstistics.EF.Migrations");
                });
        }
    }
}
