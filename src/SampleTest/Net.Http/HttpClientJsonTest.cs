using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Net.Http {
	public class HttpClientJsonTest : IDisposable {
		private class Sample {
			private int Value { get; set; }
		}

		private static class ContentTypes {
			public const string ApplicationJson = "application/json";
		}

		private class Startup {
			public void ConfigureServices(IServiceCollection services) {
			}

			private static void ConfigureGet(IApplicationBuilder app) {
				app.Run(async context => {
					if (!HttpMethods.IsGet(context.Request.Method)) {
						context.Response.StatusCode = (int)HttpStatusCode.NotFound;
						return;
					}

					// todo:
					var json = "";

					context.Response.ContentType = ContentTypes.ApplicationJson;
					await context.Response.WriteAsync(json);
				});
			}

			private static void ConfigurePost(IApplicationBuilder app) {
				app.Run(async context => {
					if (!HttpMethods.IsPost(context.Request.Method)) {
						context.Response.StatusCode = (int)HttpStatusCode.NotFound;
						return;
					}

					// todo:
					var json = "";

					context.Response.ContentType = ContentTypes.ApplicationJson;
					await context.Response.WriteAsync(json);
				});
			}

			public void Configure(IApplicationBuilder app) {
				app.Map("/get", ConfigureGet);
				app.Map("/post", ConfigurePost);

				app.Run(context => {
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					return Task.CompletedTask;
				});
			}
		}

		private readonly TestServer _server;
		private readonly HttpClient _client;
		private readonly ITestOutputHelper _output;

		public HttpClientJsonTest(ITestOutputHelper output) {
			_server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
			_client = _server.CreateClient();
			_output = output;
		}

		public void Dispose() {
			_client.Dispose();
			_server.Dispose();
		}

		[Fact(Skip = "作成中")]
		public async Task GetFromJsonAsync_使ってみる() {
			// Arrange

			// Act
			var sample = await _client.GetFromJsonAsync<Sample>("/get");

			// Assert
			// todo:
		}
	}
}
