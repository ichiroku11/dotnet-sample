using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net.Http.Json;

namespace SampleTest.Diagnostics;

public class ActivityHttpClientTest(ITestOutputHelper output) : IAsyncDisposable {
	private readonly ITestOutputHelper _output = output;
	private readonly WebApplication _app = CreateAndStartWebApp(_url);

	private class TraceResponse {
		public string TraceParent { get; init; } = "";
	}

	// todo: localhostのポート番号を固定？
	private const string _url = "http://localhost:5000";

	private static WebApplication CreateAndStartWebApp(string url) {
		var builder = WebApplication.CreateBuilder();

		var app = builder.Build();
		app.MapGet("/", async context => {
			// HTTPリクエストヘッダーの分散トレース関連の情報をJSONとして返す
			var headers = context.Request.Headers;

			var response = new TraceResponse {
				TraceParent = headers.TraceParent.ToString(),
			};

			await context.Response.WriteAsJsonAsync(response);
		});

		// todo: ポート番号
		app.Urls.Add(url);

		app.Start();

		return app;
	}

	public async ValueTask DisposeAsync() {
		await _app.StopAsync();
	}

	[Fact]
	public async Task HttpClient_TraceParentヘッダが付与されることを確認する() {
		// Arrange
		using var client = new HttpClient();

		// Act
		using var activity = new Activity("test").Start();
		_output.WriteLine(activity.Id ?? "");

		var response = await client.GetFromJsonAsync<TraceResponse>(_url);
		Assert.NotNull(response);

		var activityContext = ActivityContext.Parse(response.TraceParent, null);
		_output.WriteLine(response.TraceParent);

		// Assert
		Assert.Equal(activity.TraceId, activityContext.TraceId);
	}
}
