using AzureAppInsightsWebApp.Models;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAppInsightsWebApp {
	public class Startup {
		public void ConfigureServices(IServiceCollection services) {
			services
				// Application Insights
				// サーバ側のテレメトリを有効にする
				// https://docs.microsoft.com/ja-jp/azure/azure-monitor/app/asp-net-core#enable-application-insights-server-side-telemetry-no-visual-studio
				// おそらく以下を収集するようになる
				// - dependencies
				// - exceptions
				// - requests
				// - traces
				.AddApplicationInsightsTelemetry()
				.AddControllersWithViews();

			services.AddDbContext<AppDbContext>();

			// SQLクエリを収集する
			// dependencies.data
			// https://docs.microsoft.com/ja-jp/azure/azure-monitor/app/asp-net-dependencies#advanced-sql-tracking-to-get-full-sql-query
			services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, _) => {
				module.EnableSqlCommandTextInstrumentation = true;
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints => {
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
