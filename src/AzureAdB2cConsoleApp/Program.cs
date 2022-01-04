using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzureAdB2cConsoleApp;

class Program {
	static async Task Main(string[] args) {
		await Host
			.CreateDefaultBuilder(args)
			.ConfigureServices(services => {
				services
					.AddHostedService<SampleService>()
					.AddTransient<GraphGetUserListSample>()
					.AddTransient<GraphGetUserSample>()
					.AddTransient<GraphUpdateUserSample>();
			})
			.RunConsoleAsync();
	}
}
