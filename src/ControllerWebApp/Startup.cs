namespace ControllerWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services.AddControllers();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints => {
			endpoints.MapAreaControllerRoute(
				name: "admin",
				areaName: "Admin",
				pattern: "Admin/{controller=AdminDefault}/{action=Index}/{id?}");
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Default}/{action=Index}/{id?}");
		});
	}
}
