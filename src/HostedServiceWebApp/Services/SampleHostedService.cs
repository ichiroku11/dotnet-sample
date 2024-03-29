namespace HostedServiceWebApp.Services;

// IHostedServiceを直接実装し、Timerで定期的な処理を実行するホステッドサービス
// 参考
// https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/host/hosted-services
public class SampleHostedService(ILogger<SampleHostedService> logger) : IHostedService, IDisposable {
	private readonly ILogger _logger = logger;
	private Timer? _timer;
	private int _count;

	// 何か定期的に実行する処理
	private void Action(object? state) {
		var count = Interlocked.Increment(ref _count);

		_logger.LogInformation("{name} {count}", nameof(Action), count);

		// Timerコールバック内で例外が発生しても、次のコールバックは呼び出される
		/*
		if (count >= 5) {
			throw new Exception("Error!");
		}
		*/
	}

	public void Dispose() {
		_logger.LogInformation(nameof(Dispose));

		_timer?.Dispose();
		_timer = null;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		_logger.LogInformation(nameof(StartAsync));

		// 5秒ごとにActionを呼び出す
		_timer = new Timer(Action, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation(nameof(StopAsync));

		_timer?.Change(Timeout.Infinite, Timeout.Infinite);

		return Task.CompletedTask;
	}
}
