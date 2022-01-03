using ViewComponentWebApp.Models;

namespace ViewComponentWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services.AddControllersWithViews();

		services.AddScoped<TodoRepository>();
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
