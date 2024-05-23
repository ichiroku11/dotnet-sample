using Microsoft.Extensions.DependencyInjection;

namespace AzureAdB2cGraphConsoleApp;

internal static class ServiceProviderExtensions {
	public static Task RunSampleAsync<TSample>(this IServiceProvider services)
		where TSample : SampleBase
		=> services.GetRequiredService<TSample>().RunAsync();
}
