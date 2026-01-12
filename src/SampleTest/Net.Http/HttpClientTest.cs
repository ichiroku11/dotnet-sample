using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SampleTest.Net.Http;

public class HttpClientTest : IDisposable {
	private class Startup {
		private static void ConfigureQueryTest(IApplicationBuilder app) {
			app.Run(async context => {
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(context.Request.Query["test"].ToString());
			});
		}

		private static void ConfigureFormTest(IApplicationBuilder app) {
			app.Run(async context => {
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync(context.Request.Form["test"].ToString());
			});
		}

		public void ConfigureServices(IServiceCollection _) {
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment _) {
			app.Map("/query", ConfigureQueryTest);
			app.Map("/form", ConfigureFormTest);

			app.Run(async context => {
				// Hello World!
				context.Response.ContentType = "text/plain";
				await context.Response.WriteAsync("Hello World!");
			});
		}
	}

	private readonly IHost _host;
	private readonly HttpClient _client;

	public HttpClientTest() {
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
	public async Task GetAsync_とりあえず使ってみる() {
		// Arrange
		// Act
		var response = await _client.GetAsync("/");
		var contentType = response.Content.Headers!.ContentType!.MediaType;
		var contentText = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal("text/plain", contentType);
		Assert.Equal("Hello World!", contentText);
	}

	[Fact]
	public async Task GetAsync_クエリ文字列を使う() {
		// Arrange
		// Act
		var response = await _client.GetAsync("/query?test=xyz");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal("xyz", content);
	}

	// SendAsyncでGetAsyncと同じことをしてる
	[Fact]
	public async Task SendAsync_クエリ文字列を使う() {
		// Arrange
		var request = new HttpRequestMessage(HttpMethod.Get, "/query?test=xyz");

		// Act
		var response = await _client.SendAsync(request);
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal("xyz", content);
	}

	[Fact]
	public async Task PostAsync_フォームデータのPOSTする() {
		// Arrange
		var nameValues = new Dictionary<string, string> {
				{ "test", "abc" }
			};

		// Act
		var response = await _client.PostAsync("/form", new FormUrlEncodedContent(nameValues));
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal("abc", content);
	}

	// SendAsyncでPostAsyncと同じことをしてる
	[Fact]
	public async Task SendAsync_フォームデータのPOSTする() {
		// Arrange
		var nameValues = new Dictionary<string, string> {
				{ "test", "abc" }
			};
		var request = new HttpRequestMessage(HttpMethod.Post, "/form") {
			Content = new FormUrlEncodedContent(nameValues),
		};

		// Act
		var response = await _client.SendAsync(request);
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal("abc", content);
	}
}
