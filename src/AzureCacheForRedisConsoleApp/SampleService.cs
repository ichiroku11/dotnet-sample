using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureCacheForRedisConsoleApp;

public class SampleService : IHostedService {
	private readonly IServiceProvider _services;
	private readonly IHostApplicationLifetime _lifetime;
	private readonly ILogger _logger;

	public SampleService(
		IHost host,
		IHostApplicationLifetime lifetime,
		ILogger<SampleService> logger) {
		_services = host.Services;
		_lifetime = lifetime;
		_logger = logger;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		_logger.LogInformation(nameof(StartAsync));

		// アプリケーションの開始が完了したら
		_lifetime.ApplicationStarted.Register(async () => {
			try {
				// Redisサンプルを実行
				await _services.GetRequiredService<RedisSample>().RunAsync();
			} catch (Exception) {
				throw;
			}

			// アプリケーションを終了
			_lifetime.StopApplication();
		});

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation(nameof(StopAsync));
		return Task.CompletedTask;
	}
}
