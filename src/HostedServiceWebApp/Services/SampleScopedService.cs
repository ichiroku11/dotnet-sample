namespace HostedServiceWebApp.Services;

// Scopedで追加するサービス
public class SampleScopedService(ILogger<SampleScopedService> logger) {
	private readonly ILogger _logger = logger;

	public Task ActionAsync() {
		_logger.LogInformation($"{nameof(ActionAsync)}");
		return Task.CompletedTask;
	}
}
