using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OptionPatternWebApp {
	public class Startup {
		private readonly IConfiguration _config;

		public Startup(IConfiguration config) {
			_config = config;
		}

		public void ConfigureServices(IServiceCollection services) {
			// 設定をクラスにバインドできるようにする
			services.Configure<SampleOptions>(_config.GetSection("App:Sample"));
			// 別の方法
			//services.Configure<SampleOptions>(_config.GetSection("App").GetSection("Sample"));
			services.Configure<SampleOptionsMonitor>(_config.GetSection("App:SampleMonitor"));

			services.AddControllers();
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
}
