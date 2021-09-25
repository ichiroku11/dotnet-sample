using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostedServiceWebApp.Services {
	// Scopedで追加するサービス
	public class SampleScopedService {
		private readonly ILogger _logger;

		public SampleScopedService(ILogger<SampleScopedService> logger) {
			_logger = logger;
		}

		public Task ActionAsync() {
			_logger.LogInformation($"{nameof(ActionAsync)}");
			return Task.CompletedTask;
		}
	}
}