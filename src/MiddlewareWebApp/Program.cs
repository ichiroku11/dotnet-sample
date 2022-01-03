namespace MiddlewareWebApp;

public class Program {
	public static void Main(string[] args) {
		CreateHostBuilder(args).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder => {
					// SampleMiddleware、SampleStartupFilterを使ったStartup
					//webBuilder.UseStartup<SampleStartup>();

					// Use/UseWhen/Runを使ったStartup
					webBuilder.UseStartup<ExtensionMethodStartup>();
			});
}
