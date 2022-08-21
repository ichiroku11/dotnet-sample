// See https://aka.ms/new-console-template for more information
using AzureCosmosDbConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices(services => {
		services
			.AddHostedService<SampleService>()
			.AddTransient<CosmosSqlApiSample>()
			.AddDbContext<CosmosDbContext>()
			.AddTransient<CosmosEfCoreSample>();
	})
	.RunConsoleAsync();
