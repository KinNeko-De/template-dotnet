using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Project._1.Template._1.Operations.HealthChecks.Grpc;

public class GrpcHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        return Task.FromResult(HealthCheckResult.Healthy());
    }
}
