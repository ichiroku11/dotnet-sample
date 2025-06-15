using NCrontab;

namespace HostedServiceWebApp.Services;

// スケジュールにしたがって定期的な処理を実行する
public class SampleScheduleService(ILogger<SampleScheduleService> logger) : BackgroundService {
	private readonly ILogger<SampleScheduleService> _logger = logger;
	private int _count;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		_logger.LogInformation($"{nameof(ExecuteAsync)} Start");

		stoppingToken.Register(() => _logger.LogInformation($"{nameof(ExecuteAsync)} Canceled"));

		// 毎時5分
		var expression = "5 * * * *";

		var schedule = CrontabSchedule.Parse(expression);

		while (!stoppingToken.IsCancellationRequested) {
			var now = DateTime.Now;
			var next = schedule.GetNextOccurrence(now);

			_logger.LogInformation("Next {next}", next);

			// todo: delayがマイナスの場合を考慮する必要あり
			var delay = next - now;
			await Task.Delay(delay, stoppingToken);

			// 何か定期的に実行する処理
			var count = Interlocked.Increment(ref _count);
			_logger.LogInformation("Loop {name} = {count}", nameof(count), count);
		}

		_logger.LogInformation($"{nameof(ExecuteAsync)} End");
	}
}
