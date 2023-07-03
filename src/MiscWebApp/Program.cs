using Microsoft.AspNetCore.StaticFiles;
using MiscWebApp;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints => {
	// クライアント・サーバのIPアドレス・ポート番号を確認するEndpoint
	endpoints.MapGet("/connection", async context => {
		var connection = new {
			// サーバのIPアドレスとポート番号
			Local = new {
				IpAddress = context.Connection.LocalIpAddress?.ToString(),
				Port = context.Connection.LocalPort
			},
			// クライアントのIPアドレスとポート番号
			Remote = new {
				IpAddress = context.Connection.RemoteIpAddress?.ToString(),
				Port = context.Connection.RemotePort
			},
		};

		var json = JsonSerializer.Serialize(connection, JsonHelper.Options);
		await context.Response.WriteAsync(json);
	});

	// 拡張子からコンテンツタイプ（MIME）を取得するEndpoint
	endpoints.MapGet("contenttype/{subpath}", async context => {
		var provider = context.RequestServices.GetRequiredService<IContentTypeProvider>();

		var subpath = context.GetRouteValue("subpath") as string;
		// たぶん同じ
		//var subpath = context.Request.RouteValues["subpath"] as string;
		if (!provider.TryGetContentType(subpath ?? "", out var contentType)) {
			context.Response.StatusCode = (int)HttpStatusCode.NotFound;
			return;
		}

		await context.Response.WriteAsync(contentType);
	});

	// HTTPヘッダを確認するEndpoint
	endpoints.MapGet("/header", async context => {
		await context.Response.WriteAsync("Header");
	});

	// jsonを扱うEndpoint
	endpoints.MapPost("/json", async context => {
		// ReadFromJsonAsync/WriteAsJsonAsync - .NET 5からの機能
		// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequestjsonextensions?view=aspnetcore-5.0
		// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpresponsejsonextensions?view=aspnetcore-5.0
		// リクエストからJSONを読み込む
		var sample = await context.Request.ReadFromJsonAsync<Sample>(JsonHelper.Options);

		// レスポンスにJSONを書き込む
		await context.Response.WriteAsJsonAsync(sample, JsonHelper.Options);
	});

	// HTTPボディを確認するEndpoint
	endpoints.MapPost("/body", async context => {
		// シークできない
		var canSeek = context.Request.Body.CanSeek;
		// 1回目は読み取れる
		var first = await new StreamReader(context.Request.Body).ReadToEndAsync();
		// 2回目は読み取れない
		var second = await new StreamReader(context.Request.Body).ReadToEndAsync();

		var json = JsonSerializer.Serialize(new { canSeek, first, second }, JsonHelper.Options);
		await context.Response.WriteAsync(json);
	});

	// リクエストを確認するEndpoint
	endpoints.MapGet("/request/{**path}", async context => {
		var request = new {
			context.Request.Scheme,
			// リクエストのホスト名にポート番号が含まれる
			Host = context.Request.Host.Value,
			PathBase = context.Request.PathBase.Value,
			Path = context.Request.Path.Value,
			QueryString = context.Request.QueryString.Value,
		};
		var json = JsonSerializer.Serialize(request, JsonHelper.Options);
		await context.Response.WriteAsync(json);
	});

	endpoints.MapGet("/", async context => {
		await context.Response.WriteAsync("Hello World!");
	});
});

app.Run();

// MiscWebApp.Testのため
public partial class Program {
}
