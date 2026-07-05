using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace SampleTest.AspNetCore.Http.Features;

public class HttpActivityFeatureTest(ITestOutputHelper output) : IAsyncDisposable {
	private readonly ITestOutputHelper _output = output;
	private readonly WebApplication _app = CreateAndStartWebApp();

	private static WebApplication CreateAndStartWebApp() {
		var builder = WebApplication.CreateBuilder();
		// TestServerでもActivityFeatureからActivityを取得できる
		builder.WebHost.UseTestServer();

		var app = builder.Build();

		app.MapGet("/", async context => {
			var feature = context.Features.Get<IHttpActivityFeature>();
			var activity = feature?.Activity;

			await context.Response.WriteAsync(activity?.Id ?? "");
		});

		app.Start();

		return app;
	}

	public async ValueTask DisposeAsync() {
		await _app.StopAsync();

		GC.SuppressFinalize(this);
	}

	[Fact]
	public async Task サーバー側で取得できるActivityを確認する() {
		// Arrange
		using var client = _app.GetTestClient();

		// Act
		var response = await client.GetAsync("/");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotEmpty(content);
		_output.WriteLine(content);
	}
}
