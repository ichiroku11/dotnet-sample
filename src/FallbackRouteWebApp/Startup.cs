using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
