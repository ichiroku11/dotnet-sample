// See https://aka.ms/new-console-template for more information
using AzureCosmosDbConsoleApp;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) => {
		var config = context.Configuration;
		services
			.AddHostedService<SampleService>()
			.AddSingleton(provider => {
				return new CosmosClientBuilder(config.GetConnectionString(Constants.ConnectionStringName))
					.WithSerializerOptions(new CosmosSerializationOptions {
						PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
					})
					.Build();
			})
			.AddTransient<CosmosSqlApiSample>()
			.AddDbContext<CosmosDbContext>()
			.AddTransient<CosmosEfCoreSample>();
	})
	.RunConsoleAsync();
