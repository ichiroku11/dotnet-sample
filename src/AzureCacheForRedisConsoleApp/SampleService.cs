using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleLib.Hosting;

namespace AzureCacheForRedisConsoleApp;

public class SampleService(IHost host, IHostApplicationLifetime lifetime, ILogger<SampleService> logger)
	: OnceHostedService(host, lifetime, logger) {
	protected override Task RunAsync(IServiceProvider services) {
		return services.GetRequiredService<RedisSample>().RunAsync();
	}
}
