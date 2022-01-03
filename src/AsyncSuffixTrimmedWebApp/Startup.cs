namespace AsyncSuffixTrimmedWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services.AddControllersWithViews();

		services.Configure<RouteOptions>(options => {
			options.LowercaseQueryStrings = true;
			options.LowercaseUrls = true;
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints => {
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Default}/{action=Index}/{id?}");
		});
	}
}
