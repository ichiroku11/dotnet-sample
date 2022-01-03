using Microsoft.AspNetCore.Mvc;

namespace AntiForgeryWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services.AddControllersWithViews(options => {
			// GET/HEAD/OPTIONS/TRACE以外にのメソッドに対してトークンの検証を行う
			options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
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
