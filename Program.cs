//using DigitalBanking.Services.Contracts;
//using DigitalBanking.Services.Implementation;
//using Serilog;
//using Serilog.Context;

using DigitalBanking.ServiceExtensions;
using DigitalBanking.Services.Contracts;
using DigitalBanking.Services.Implementation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace DigitalBanking.Global
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCarter();
            //builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));

            builder.AddSwagger();
            builder.AddSerilog();
            builder.Services.AddSingleton(Log.Logger);
            builder.Services.AddTransient<IOTPService, OneTimePinService>();

            builder.Services.AddHostedService<StartupBackgroundService>();
            builder.Services.AddSingleton<StartupHealthCheck>();
            builder.Services.AddHealthChecks()
                .AddCheck<StartupHealthCheck>(
                    "Startup",
                    tags: new[] { "ready" })
                //.AddOracle(
                //    builder.Configuration.GetConnectionString("DefaultConnection"));
                .AddCheck<SampleHealthCheck>("Sample", tags: new[] { "live" });



            var app = builder.Build();
            //app.UseEndpointDefinitions();
            app.UseMiddleware<RequestLogContextMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app
                .UseExceptionHandling(app.Environment)
                .UseSwaggerEndpoints(routePrefix: string.Empty);

            app.MapGet("/", () => "Hello World!");
            app.MapCarter();


            //register all services then do a health check

            app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
            {
                Predicate = healthCheck => healthCheck.Tags.Contains("ready")
            });

            app.MapHealthChecks("/healthz/live", new HealthCheckOptions
            {
                Predicate = healthCheck => healthCheck.Tags.Contains("live")
            });


            app.Run();







        }
    }
}