// See https://aka.ms/new-console-template for more information
using AzureAdB2cUserManagementConsoleApp;
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
			.AddTransient<GraphUpdateUserForceChangePasswordSample>();
	})
	.RunConsoleAsync();

