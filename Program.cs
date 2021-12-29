//using DigitalBanking.Services.Contracts;
//using DigitalBanking.Services.Implementation;
//using Serilog;
//using Serilog.Context;

using DigitalBanking.ServiceExtensions;
using DigitalBanking.Services.Contracts;
using DigitalBanking.Services.Implementation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
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







