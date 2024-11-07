using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleLib.Hosting;

namespace AzureCosmosDbConsoleApp;

public class SampleService(IHost host, IHostApplicationLifetime lifetime, ILogger<SampleService> logger)
	: OnceHostedService(host, lifetime, logger) {
	protected override async Task RunAsync(IServiceProvider services) {
		await services.GetRequiredService<CosmosSqlApiSample>().RunAsync();

		/*
		// DBコンテキストにはスコープが必要っぽい
		// （AddDbContextメソッドを呼び出すときに調整することもできそう）
		using var scope = services.CreateScope();
		await scope.ServiceProvider.GetRequiredService<CosmosEfCoreSample>().RunAsync();
		*/
	}
}
