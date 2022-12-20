using Feedstistics.Api;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureLogging(builder =>
    {
        builder.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
    })
    .ConfigureOpenApi()
    .ConfigureServices(ConfigureServices.ConfigureFunctionServices)
    .UseDefaultServiceProvider((context, options) =>
    {
        if (context.HostingEnvironment.IsDevelopment())
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        }
    })
    .Build();

        host.Run();
    }
}