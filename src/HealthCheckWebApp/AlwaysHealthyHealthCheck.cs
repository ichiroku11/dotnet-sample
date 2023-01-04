using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckWebApp;

// 常にHealthy
public class AlwaysHealthyHealthCheck : IHealthCheck {
	public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		=> Task.FromResult(HealthCheckResult.Healthy(description: "Always healthy"));
}
