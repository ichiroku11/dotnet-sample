using NCrontab;

namespace HostedServiceWebApp.Services;

// スケジュールにしたがって定期的な処理を実行する
public class SampleScheduleService(IConfiguration config, ILogger<SampleScheduleService> logger) : BackgroundService {
	private readonly IConfiguration _config = config;
	private readonly ILogger<SampleScheduleService> _logger = logger;
	private int _count;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		_logger.LogInformation($"{nameof(ExecuteAsync)} Start");

		stoppingToken.Register(() => _logger.LogInformation($"{nameof(ExecuteAsync)} Canceled"));

		// 毎時5分
		var expression = _config["Schedule:Sample"];
		var schedule = CrontabSchedule.Parse(expression);

		while (!stoppingToken.IsCancellationRequested) {
			var now = DateTime.Now;
			var next = schedule.GetNextOccurrence(now);

			// NCrontabで取得できる次の日時は未来の日時になる様子だが、
			// マイナスの値だと例外が発生し、また万が一-1msを指定すると無限に待機してしまうため、マイナスは除外する
			var delay = next - now;
			if (delay < TimeSpan.Zero) {
				continue;
			}

			_logger.LogInformation("Next {next}", next);

			await Task.Delay(delay, stoppingToken);

			// 何か定期的に実行する処理
			var count = Interlocked.Increment(ref _count);
			_logger.LogInformation("Loop {name} = {count}", nameof(count), count);
		}

		_logger.LogInformation($"{nameof(ExecuteAsync)} End");
	}
}
