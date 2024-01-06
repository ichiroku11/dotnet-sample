using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleLib.Hosting;

// 一度実行したら終了するサービス
public abstract class OnceHostedService(
	IHost host,
	IHostApplicationLifetime lifetime,
	ILogger logger) : IHostedService {
	private readonly IServiceProvider _services = host.Services;
	private readonly IHostApplicationLifetime _lifetime = lifetime;
	private readonly ILogger _logger = logger;

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
