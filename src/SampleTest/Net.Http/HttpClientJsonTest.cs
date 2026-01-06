using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Http.Json;

namespace SampleTest.Net.Http;

// GetFromJsonAsync/PostFromJsonAsync/PutFromJsonAsync
// 参考
// https://docs.microsoft.com/ja-jp/dotnet/api/system.net.http.json.httpclientjsonextensions?view=net-5.0
// https://docs.microsoft.com/ja-jp/dotnet/api/system.net.http.json.httpcontentjsonextensions?view=net-5.0
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

		private static void ConfigurePut(IApplicationBuilder app) {
			app.Run(async context => {
				if (!HttpMethods.IsPut(context.Request.Method)) {
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
			app.Map("/put", ConfigurePut);

			app.Run(context => {
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return Task.CompletedTask;
			});
		}
	}

	private readonly IHost _host;
	private readonly HttpClient _client;

	public HttpClientJsonTest() {
		_host = new HostBuilder()
			.ConfigureWebHost(builder => {
				builder
					.UseTestServer()
					.UseStartup<Startup>();
			})
			.Start();
		_client = _host.GetTestClient();
	}

	public void Dispose() {
		_client.Dispose();
		_host.Dispose();

		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task GetFromJsonAsync_使ってみる() {
		// Arrange
		// Act
		var sample = await _client.GetFromJsonAsync<Sample>("/get");

		// Assert
		Assert.Equal(1, sample!.Value);
	}

	[Fact]
	public async Task PostAsJsonAsync_使ってみる() {
		// Arrange
		// Act
		var response = await _client.PostAsJsonAsync("/post", new Sample { Value = 2 });
		var sample = await response.Content.ReadFromJsonAsync<Sample>();

		// Assert
		Assert.Equal(2, sample!.Value);
	}

	[Fact]
	public async Task PutAsJsonAsync_使ってみる() {
		// Arrange
		// Act
		var response = await _client.PutAsJsonAsync("/put", new Sample { Value = 3 });
		var sample = await response.Content.ReadFromJsonAsync<Sample>();

		// Assert
		Assert.Equal(3, sample!.Value);
	}

}
