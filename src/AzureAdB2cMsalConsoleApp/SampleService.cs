using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureAdB2cMsalConsoleApp;

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
				// todo:
				Console.WriteLine("Hello");

				await Task.CompletedTask;
			} catch (Exception exception) {
				_logger.LogError(exception, nameof(IHostApplicationLifetime.ApplicationStarted));
			} finally {
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
