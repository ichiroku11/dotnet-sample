using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureAdB2cConsoleApp {
	public class SampleService : IHostedService {
		private readonly IServiceProvider _services;
		private readonly IHostApplicationLifetime _lifetime;
		private readonly ILogger _logger;

		public SampleService(
			IHost host,
			IHostApplicationLifetime lifetime,
			ILogger<SampleService> logger) {
			_services = host.Services;
			_lifetime = lifetime;
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken) {
			_logger.LogInformation(nameof(StartAsync));

			_lifetime.ApplicationStarted.Register(async () => {
				try {
					await _services.GetRequiredService<GraphSample>().RunAsync();
				} catch (Exception) {
					throw;
				}

				_lifetime.StopApplication();
			});

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			_logger.LogInformation(nameof(StopAsync));
			return Task.CompletedTask;
		}
	}
}
