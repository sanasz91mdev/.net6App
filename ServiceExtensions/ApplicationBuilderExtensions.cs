namespace Microsoft.AspNetCore.Builder;

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Primitives;
using Serilog.Context;

internal static partial class ApplicationBuilderExtensions
{
    /// <summary>
    /// Register exception handling.
    /// </summary>
    public static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        return app;
    }

    ///// <summary>
    ///// Register CORS.
    ///// </summary>
    //public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
    //{
    //    app.UseCors(p =>
    //    {
    //        p.AllowAnyOrigin();
    //        p.WithMethods("GET");
    //        p.AllowAnyHeader();
    //    });

    //    return app;
    //}
}


public static class ContectExtension
{
    public static string GetCorrelationId(this HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue("Cko-Correlation-Id", out StringValues correlationId);
        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}

public class RequestLogContextMiddleware
{
    private readonly RequestDelegate _next;



    public RequestLogContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    //public Task Invoke(HttpContext context)
    //{
    //    var correlatoionId = context.GetCorrelationId();
    //    using (LogContext.PushProperty("CorrelationId", correlatoionId))
    //    {
    //        return _next.Invoke(context);
    //    }
    //}


    public Task Invoke(HttpContext context, ILogger<RequestLogContextMiddleware> logger)
    {
        var dictionary = new Dictionary<string, object>
    {
         { "CorrelationId", context.GetCorrelationId() },
         { "MachineName", Environment.MachineName}
    };

        // ensures all entries are tagged with some values
        using (logger.BeginScope(dictionary))
        {
            logger.LogInformation("Logging delegate");
            // Call the next delegate/middleware in the pipeline
            return _next(context);
        }
    }

}


public class StartupBackgroundService : BackgroundService
{
    private readonly StartupHealthCheck _healthCheck;

    public StartupBackgroundService(StartupHealthCheck healthCheck)
        => _healthCheck = healthCheck;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Simulate the effect of a long-running task. //Must use this
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

        _healthCheck.StartupCompleted = true;
    }
}

public class StartupHealthCheck : IHealthCheck
{
    private volatile bool _isReady;

    public bool StartupCompleted
    {
        get => _isReady;
        set => _isReady = value;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (StartupCompleted)
        {
            return Task.FromResult(HealthCheckResult.Healthy("The startup task has completed."));
        }

        return Task.FromResult(HealthCheckResult.Unhealthy("That startup task is still running."));
    }
}


