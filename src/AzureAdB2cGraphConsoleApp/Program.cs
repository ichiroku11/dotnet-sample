using AzureAdB2cGraphConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) => {
		var config = context.Configuration;
		services
			.AddHostedService<SampleService>()
			.Configure<GraphServiceOptions>(config.GetSection("AzureAdB2cGraph"))
			.AddSingleton<GraphServiceClientFactory>()
			.AddSingleton(provider => provider.GetRequiredService<GraphServiceClientFactory>().Create())
			.AddTransient<ApplicationGetListPagingSample>()
			.AddTransient<ApplicationGetListSample>()
			.AddTransient<DirectoryAuditGetListSample>()
			.AddTransient<UserCreateSample>()
			.AddTransient<UserGetListSample>()
			.AddTransient<UserGetListSignInActivitySample>()
			.AddTransient<UserGetSample>()
			.AddTransient<UserUpdateAccountEnabledSample>()
			.AddTransient<UserUpdateCustomAttributeSample>()
			.AddTransient<UserUpdateForceChangePasswordSample>()
			.AddTransient<UserUpdateSignInMailAddressSample>();
	})
	.RunConsoleAsync();
