using AzureAdB2cGraphConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
			.AddTransient<GraphUpdateUserForceChangePasswordSample>()
			.AddTransient<GraphUpdateUserSignInMailAddressSample>();
	})
	.RunConsoleAsync();
