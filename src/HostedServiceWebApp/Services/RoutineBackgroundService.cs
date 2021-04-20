using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceWebApp.Services {
	// BackgroundServiceを継承し、定期的な処理を実行するホステッドサービス
	public class RoutineBackgroundService : BackgroundService {
		private readonly ILogger _logger;

		public RoutineBackgroundService(ILogger<RoutineBackgroundService> logger) {
			_logger = logger;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken) {
			_logger.LogInformation(nameof(ExecuteAsync));

			// todo:
			return Task.CompletedTask;
		}
	}
}
