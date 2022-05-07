using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleLib.Hosting;

// 一度実行したら終了するサービス
public abstract class OnceHostedService : IHostedService {
	private readonly IServiceProvider _services;
	private readonly IHostApplicationLifetime _lifetime;
	private readonly ILogger _logger;

	public OnceHostedService(
		IHost host,
		IHostApplicationLifetime lifetime,
		ILogger logger) {
		_services = host.Services;
		_lifetime = lifetime;
		_logger = logger;
	}

	// 実行
	protected abstract Task RunAsync(IServiceProvider services);

	public Task StartAsync(CancellationToken cancellationToken) {
		_logger.LogInformation(nameof(StartAsync));

		// アプリケーションの開始が完了したら
		_lifetime.ApplicationStarted.Register(async () => {
			try {
				// メイン処理を実行
				await RunAsync(_services);

			} catch (Exception exception) {
				_logger.LogError(exception, nameof(IHostApplicationLifetime.ApplicationStarted));
			} finally {
				// アプリケーションを終了
				_lifetime.StopApplication();
			}
		});

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation(nameof(StopAsync));
		return Task.CompletedTask;
	}
}
