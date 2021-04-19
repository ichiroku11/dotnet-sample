using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceWebApp.Services {
	// Timerで定期的な処理を実行する
	// IHostedServiceを直接使う
	// 参考
	// https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/host/hosted-services
	public class TimerHostedService : IHostedService {
		private readonly ILogger _logger;
		public TimerHostedService(ILogger<TimerHostedService> logger) {
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken) {
			_logger.LogInformation(nameof(StartAsync));

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			_logger.LogInformation(nameof(StopAsync));

			return Task.CompletedTask;
		}
	}
}
