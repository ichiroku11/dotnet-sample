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

namespace EndpointWebApp {
	// エンドポイントの表示名やメタデータを確認する、MVCを使わないサンプルのStartup
	public class SampleStartup {
		public void ConfigureServices(IServiceCollection services) {
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints => {
				endpoints
					.MapGet("/metadata", async context => {
						// UseRouting以降のミドルウェアであればエンドポイントを取得できる
						var endpoint = context.GetEndpoint();
						await context.Response.WriteLineAsync(endpoint.DisplayName);

						// エンドポイントに関連するメタデータを取得
						var metadata = endpoint.Metadata.GetMetadata<ISampleMetadata>();
						await context.Response.WriteLineAsync($"{nameof(ISampleMetadata)}:{metadata.Value}");
					})
					.WithDisplayName("metadata")
					// メタデータを指定する
					.WithMetadata(new SampleMetadataAttribute(value: 1));

				endpoints
					.MapGet("/endpoints", async context => {
						var endpointDataSource = context.RequestServices.GetRequiredService<EndpointDataSource>();

						// エンドポイント一覧
						foreach (var endpoint in endpointDataSource.Endpoints) {
							await context.Response.WriteLineAsync(endpoint.DisplayName);
						}
					})
					.WithDisplayName("endpoints");

				endpoints
					.MapGet("/", async context => {
						await context.Response.WriteAsync("Hello World!");
					}).WithDisplayName("root");
			});
		}
	}
}
