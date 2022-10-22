using Grpc.Core;
using Grpc.Health.V1;

namespace Project._1.Template._1.Operations.HealthChecks.Grpc;

public class GrpcHealthCheck : Health.HealthBase
{
    public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
    {
        var serviceStatus = request.Service is "" or "healthy" ? HealthCheckResponse.Types.ServingStatus.Serving : HealthCheckResponse.Types.ServingStatus.Unknown;

        return Task.FromResult(new HealthCheckResponse()
        {
            Status = serviceStatus
        });
    }
}
