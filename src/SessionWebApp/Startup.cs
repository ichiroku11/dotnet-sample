using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SessionWebApp {
	public class Startup {
		public void ConfigureServices(IServiceCollection services) {
			services
				.AddDistributedMemoryCache(options => {
				})
				.AddSession(options => {
					options.Cookie.Name = "session";
				});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseSession();

			app.UseEndpoints(endpoints => {
				// セッション一覧
				endpoints.MapGet("/session", async context => {
					var session = context.Session;
					foreach (var key in session.Keys) {
						await context.Response.WriteAsync($"{key}: {session.GetString(key)}");
						await context.Response.WriteAsync(Environment.NewLine);
					}
				});

				// セッションに設定
				endpoints.MapGet("/session/set/{key}:{value}", async context => {
					var session = context.Session;
					var key = context.Request.RouteValues["key"] as string;
					var value = context.Request.RouteValues["value"] as string;

					session.SetString(key, value);

					await context.Response.WriteAsync($"{key}: {session.GetString(key)}");
				});

				// セッションから削除
				endpoints.MapGet("/session/remove/{key}", async context => {
					var session = context.Session;
					var key = context.Request.RouteValues["key"] as string;

					session.Remove(key);

					await context.Response.WriteAsync($"{key}:");
				});

				endpoints.MapGet("/", async context => {
					await context.Response.WriteAsync("Hello World!");
				});
			});
		}
	}
}
