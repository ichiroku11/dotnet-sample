namespace FallbackRouteWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services.AddControllers();
		services.Configure<RouteOptions>(options => {
			options.LowercaseUrls = true;
			options.LowercaseQueryStrings = true;
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		// これのほうがよさげな
		//app.UseStatusCodePagesWithReExecute("/error/{0}");
		//app.UseStatusCodePagesWithReExecute("/error/notfound");

		app.UseRouting();

		app.UseEndpoints(endpoints => {
			// デフォルトルート
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Default}/{action=Index}/{id?}");

			// フォールバックルート
			// 未定義のアクション（URL）にアクセスされると
			// 下記コントローラ・アクションが呼び出される様子
			endpoints.MapFallbackToController(
				controller: "Error",
				action: "NotFound");
		});
	}
}
