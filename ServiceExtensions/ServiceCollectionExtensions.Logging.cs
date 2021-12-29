namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .WriteTo.Async(a => a.File("log.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}", rollingInterval: RollingInterval.Day))
            .WriteTo.SpectreConsole(
                "{Timestamp:HH:mm:ss} [{Level:u4}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}",
                minLevel: LogEventLevel.Information)
            //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} [{CorrelationId}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();


        builder.Host.UseSerilog();

        return builder;
    }
}
