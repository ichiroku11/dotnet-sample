using AzureStorageConsoleApp;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) => {
		var config = context.Configuration;
		services
			.AddHostedService<SampleService>()
			.AddSingleton(provider => new BlobServiceClient(config.GetConnectionString("Storage")))
			.AddTransient<BlobSample>();
	})
	.RunConsoleAsync();
