using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Project._1.Template._1.Operations.HealthChecks.Diagnostics;

public class HttpHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}
