using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DigitalBanking.ServiceExtensions
{

        public class DatabaseHealthCheck : IHealthCheck
        {
            public Task<HealthCheckResult> CheckHealthAsync(
                HealthCheckContext context, CancellationToken cancellationToken = default)
            {
                var isHealthy = true;

                // ...

                if (isHealthy)
                {
                //Probe database ?
                    return Task.FromResult(
                        HealthCheckResult.Healthy("Database is healthy."));
                }

                return Task.FromResult(
                    new HealthCheckResult(
                        context.Registration.FailureStatus, "Database is unhealthy."));
            }
        }
    
}
