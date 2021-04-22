using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceWebApp.Services {
	// BackgroundServiceを継承し、定期的な処理を実行するホステッドサービス
	public class SampleBackgroundService : BackgroundService {
		private readonly ILogger _logger;
		private int _count;

		public SampleBackgroundService(ILogger<SampleBackgroundService> logger) {
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			_logger.LogInformation($"{nameof(ExecuteAsync)} Start");

			stoppingToken.Register(() => _logger.LogInformation($"{nameof(ExecuteAsync)} Canceled"));

			while (!stoppingToken.IsCancellationRequested) {
				// 何か定期的に実行する処理
				var count = Interlocked.Increment(ref _count);
				_logger.LogInformation($"Loop {nameof(count)} = {count}");

				// 5秒待機
				await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
			}

			_logger.LogInformation($"{nameof(ExecuteAsync)} End");
		}
	}
}
