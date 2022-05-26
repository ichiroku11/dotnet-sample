// https://docs.microsoft.com/ja-jp/azure/active-directory-b2c/integrate-with-app-code-samples
// https://github.com/Azure-Samples/active-directory-b2c-dotnet-desktop

using AzureAdB2cMsalConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host
	.CreateDefaultBuilder(args)
	.ConfigureServices(services => {
		services
			.AddHostedService<SampleService>()
			.AddTransient<AcquireTokenInteractiveSample>();

		services
			.AddHttpClient()
			.AddTransient<LoggingDelegatingHandler>();

		services
			.AddHttpClient("b2c")
			.AddHttpMessageHandler<LoggingDelegatingHandler>();

	})
	.RunConsoleAsync();
