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
using ModelBindingWebApp.Models;

namespace ModelBindingWebApp {
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
}
