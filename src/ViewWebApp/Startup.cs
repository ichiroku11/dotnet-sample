namespace ViewWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services.AddRazorPages();

		services.AddControllersWithViews();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints => {
			endpoints.MapRazorPages();

			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Default}/{action=Index}/{id?}");
		});
	}
}
