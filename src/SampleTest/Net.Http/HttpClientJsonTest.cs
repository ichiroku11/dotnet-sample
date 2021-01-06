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
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Net.Http {
	public class HttpClientJsonTest : IDisposable {
		private class Sample {
			public int Value { get; init; }
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

					context.Response.ContentType = "application/json";
					await context.Response.WriteAsJsonAsync(new Sample { Value = 1 });
				});
			}

			private static void ConfigurePost(IApplicationBuilder app) {
				app.Run(async context => {
					if (!HttpMethods.IsPost(context.Request.Method)) {
						context.Response.StatusCode = (int)HttpStatusCode.NotFound;
						return;
					}

					var sample = await context.Request.ReadFromJsonAsync<Sample>();

					context.Response.ContentType = "application/json";
					await context.Response.WriteAsJsonAsync(sample);
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

		[Fact]
		public async Task GetFromJsonAsync_使ってみる() {
			// Arrange
			// Act
			var sample = await _client.GetFromJsonAsync<Sample>("/get");

			// Assert
			Assert.Equal(1, sample.Value);
		}

		[Fact]
		public async Task PostAsJsonAsync_使ってみる() {
			// Arrange
			// Act
			var response = await _client.PostAsJsonAsync("/post", new Sample { Value = 2 });
			var sample = await response.Content.ReadFromJsonAsync<Sample>();

			// Assert
			Assert.Equal(2, sample.Value);
		}

	}
}
