using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleLib.Hosting;

namespace AzureCosmosDbConsoleApp;

public class SampleService : OnceHostedService {
	public SampleService(IHost host, IHostApplicationLifetime lifetime, ILogger<SampleService> logger)
		: base(host, lifetime, logger) {
	}

	protected override async Task RunAsync(IServiceProvider services) {
		/*
		return services.GetRequiredService<CosmosSqlApiSample>().RunAsync();
		*/

		// DBコンテキストにはスコープが必要っぽい
		// （AddDbContextメソッドを呼び出すときに調整することもできそう）
		using var scope = services.CreateScope();
		await scope.ServiceProvider.GetRequiredService<CosmosEfCoreSample>().RunAsync();
	}
}
