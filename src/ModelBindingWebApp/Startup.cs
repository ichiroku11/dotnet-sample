using ModelBindingWebApp.Models;

namespace ModelBindingWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services
			.AddControllersWithViews(options => {
				options.ModelBinderProviders.Insert(0, new GeometryModelBinderProvider());
			})
			.AddViewOptions(options => {
					// クライアント側バリーデーションを無効にする
					options.HtmlHelperOptions.ClientValidationEnabled = false;
			});

		// セッションを使うなら
		/*
		services
			.AddControllersWithViews()
			// TempDataはセッションを使う
			.AddSessionStateTempDataProvider();

		services.AddSession();
		*/

		services.Configure<RouteOptions>(options => {
			options.LowercaseUrls = true;
			options.LowercaseQueryStrings = true;
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		// セッションを使うなら
		//app.UseSession();

		app.UseEndpoints(endpoints => {
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Default}/{action=Index}/{id?}");
		});
	}
}
