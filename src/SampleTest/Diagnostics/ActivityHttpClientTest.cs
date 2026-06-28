using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net.Http.Json;

namespace SampleTest.Diagnostics;

public class ActivityHttpClientTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	// todo: localhostのポート番号を固定？
	private const string _url = "http://localhost:5000";

	private class TraceResponse {
		public string TraceParent { get; init; } = "";
	}

	private static WebApplication CreateWebApp(string url) {
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

		return app;
	}

	[Fact]
	public async Task HttpClient_TraceParentヘッダが付与されることを確認する() {
		// Arrange
		// todo: appをプライベート変数にして、テストの開始と終了でStartAsyncとStopAsyncを呼び出すようにする
		var app = CreateWebApp(_url);
		await app.StartAsync();

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

		// todo:
		await app.StopAsync();
	}
}
