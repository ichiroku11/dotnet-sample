using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net.Http.Json;

namespace SampleTest.Diagnostics;

[Collection(CollectionNames.DotNetActivity)]
public class ActivityHttpClientTest(ITestOutputHelper output) : IAsyncDisposable {
	private readonly ITestOutputHelper _output = output;
	private readonly WebApplication _app = CreateAndStartWebApp(_url);

	private class TraceResponse {
		public string Baggage { get; init; } = "";
		public string TraceParent { get; init; } = "";
		public string TraceState { get; init; } = "";
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
				Baggage = headers.Baggage.ToString(),
				TraceParent = headers.TraceParent.ToString(),
				TraceState = headers.TraceState.ToString(),
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
	public async Task HttpClient_TraceParentヘッダーが付与されることを確認する() {
		// Arrange
		using var client = new HttpClient();

		// Act
		using var activity = new Activity("test").Start();
		_output.WriteLine(activity.Id ?? "");

		var response = await client.GetFromJsonAsync<TraceResponse>(_url);

		// Assert
		Assert.NotNull(response);
		Assert.Empty(response.Baggage);
		Assert.NotEmpty(response.TraceParent);
		Assert.Empty(response.TraceState);

		_output.WriteLine(response.TraceParent);

		var activityContext = ActivityContext.Parse(response.TraceParent, null);
		Assert.Equal(activity.TraceId, activityContext.TraceId);
	}

	[Fact]
	public async Task HttpClient_TraceStateヘッダーが付与されることを確認する() {
		// Arrange
		using var client = new HttpClient();

		// Act
		using var activity = new Activity("test").Start();
		_output.WriteLine(activity.Id ?? "");
		activity.TraceStateString = "test=1";

		var response = await client.GetFromJsonAsync<TraceResponse>(_url);

		// Assert
		Assert.NotNull(response);
		Assert.Empty(response.Baggage);
		Assert.NotEmpty(response.TraceParent);
		Assert.NotEmpty(response.TraceState);

		_output.WriteLine(response.TraceState);
		Assert.Equal("test=1", response.TraceState);
	}

	[Fact]
	public async Task HttpClient_Baggageヘッダーが付与されることを確認する() {
		// Arrange
		using var client = new HttpClient();

		// Act
		using var activity = new Activity("test").Start();
		_output.WriteLine(activity.Id ?? "");
		activity.AddBaggage("key1", "value1");
		activity.AddBaggage("key2", "value2");

		var response = await client.GetFromJsonAsync<TraceResponse>(_url);

		// Assert
		Assert.NotNull(response);
		Assert.NotEmpty(response.Baggage);
		Assert.NotEmpty(response.TraceParent);
		Assert.Empty(response.TraceState);

		// key/valueの順番は保証されない様子？
		_output.WriteLine(response.Baggage);
		Assert.Contains("key1 = value1", response.Baggage);
		Assert.Contains("key2 = value2", response.Baggage);
	}
}
