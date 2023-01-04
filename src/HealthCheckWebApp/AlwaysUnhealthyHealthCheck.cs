using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckWebApp;

// 常にUnhealthy
public class AlwaysUnhealthyHealthCheck : IHealthCheck {
	public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		=> Task.FromResult(HealthCheckResult.Unhealthy(description: "Always unhealthy"));
}
