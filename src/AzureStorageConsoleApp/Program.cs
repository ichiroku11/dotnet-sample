using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzureStorageConsoleApp;

class Program {
	static async Task Main(string[] args) {
		await Host
			.CreateDefaultBuilder(args)
			.ConfigureServices(services => {
				services
					.AddHostedService<SampleService>()
					.AddTransient<BlobSample>();
			})
			.RunConsoleAsync();
	}
}
