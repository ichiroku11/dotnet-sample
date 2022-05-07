using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleLib.Hosting;

namespace AzureAdB2cMsalConsoleApp;


public class SampleService : OnceHostedService {
	public SampleService(IHost host, IHostApplicationLifetime lifetime, ILogger<SampleService> logger)
		:  base(host, lifetime, logger) {
	}

	protected override Task RunAsync(IServiceProvider services) {
		Console.WriteLine("Sample");
		return Task.CompletedTask;
	}
}
