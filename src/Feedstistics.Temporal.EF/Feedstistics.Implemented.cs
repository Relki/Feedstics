namespace Feedstistics.EF.Models.Context
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Feedstistics Context with partial code implementing extendability.
    /// </summary>
    public partial class FeedstisticsModelContext : DbContext
    {
        partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigureOptions(optionsBuilder);
        }

        public static void ConfigureOptions(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Trace);

            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile($"appsettings.Development.json", true)
                .Build();

            var connectionString = configuration.GetConnectionString(nameof(FeedstisticsModelContext))
                ??
                configuration.GetValue<string>(nameof(FeedstisticsModelContext));

            optionsBuilder.UseSqlServer(connectionString, providerOptions =>
            {
                providerOptions.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                providerOptions.MigrationsAssembly("Feedstistics.EF.Migrations");
            });
        }

        partial void OnModelCreatingDataSeeding(ModelBuilder modelBuilder);
    }
}
