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
					.AddTransient<GraphCreateUserSample>()
					.AddTransient<GraphGetUserListSample>()
					.AddTransient<GraphGetUserSample>()
					.AddTransient<GraphUpdateUserAccountEnabledSample>()
					.AddTransient<GraphUpdateUserCustomAttributeSample>()
					.AddTransient<GraphUpdateUserForceChangePasswordSample>();
			})
			.RunConsoleAsync();
	}
}
