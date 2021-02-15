using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MiddlewareWebApp {
	// SampleMiddleware、SampleStartupFilterを使ったStartup
	public class SampleStartup {
		public void ConfigureServices(IServiceCollection services) {
			services.AddControllers();

			// IStartupFilterでミドルウェアを登録する
			services.AddTransient<IStartupFilter, SampleStartupFilter>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseSample("BeforeRouting");

			app.UseRouting();

			app.UseSample("AfterRouting");
			//app.UseMiddleware<SampleMiddleware>();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Default}/{action=Index}/{id?}");
			});
		}
	}
}
