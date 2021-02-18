using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DistributedCacheWebApp {
	public class Startup {
		private readonly IConfiguration _config;

		public Startup(IConfiguration config) {
			_config = config;
		}

		public void ConfigureServices(IServiceCollection services) {
			// セッションのサービスを追加
			services.AddSession(options => {
				// セッションクッキーの名前を変える
				options.Cookie.Name = "session";
			});

			// SQL Server分散キャッシュのサービスを追加
			services.AddDistributedSqlServerCache(options => {
				options.ConnectionString = _config.GetConnectionString("Cache");
				options.SchemaName = "dbo";
				options.TableName = "AppCache";
			});

			// コントローラ
			services.AddControllers();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			// セッション
			app.UseSession();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action=Get}/{value?}");
			});
		}
	}
}
