using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenericHostOnceConsoleApp {
	// バックグラウンドで実行されるサービス
	class SampleService : IHostedService {
		private readonly ILogger _logger;

		public SampleService(ILogger<SampleService> logger) {
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
