using AzureAdB2cGraphConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices(services => {
		services
			.AddHostedService<SampleService>()
			.AddTransient<ApplicationGetListPagingSample>()
			.AddTransient<ApplicationGetListSample>()
			.AddTransient<DirectoryAuditGetListSample>()
			.AddTransient<UserCreateSample>()
			.AddTransient<UserGetListSample>()
			.AddTransient<UserGetSample>()
			.AddTransient<UserUpdateAccountEnabledSample>()
			.AddTransient<UserUpdateCustomAttributeSample>()
			.AddTransient<UserUpdateForceChangePasswordSample>()
			.AddTransient<UserUpdateSignInMailAddressSample>();
	})
	.RunConsoleAsync();
