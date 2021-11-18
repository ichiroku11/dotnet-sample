using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace AzureAdB2cConsoleApp;

class Program {
	static async Task Main(string[] args) {
		await Host
			.CreateDefaultBuilder(args)
			.ConfigureServices(services => {
				services
					.AddHostedService<SampleService>()
					.AddTransient<GraphSample>();
			})
			.RunConsoleAsync();
	}
}
