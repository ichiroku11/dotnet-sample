using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http.Features;
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

// HTTPボディを確認するEndpoint
app.MapPost("/body", async context => {
	if (context.Request.Query.TryGetValue("buffering", out var value) &&
		bool.TryParse(value, out var enabled) &&
		enabled) {
		context.Request.EnableBuffering();
	}

	var canSeek = context.Request.Body.CanSeek;

	var first = await new StreamReader(context.Request.Body, leaveOpen: true).ReadToEndAsync();

	var thrown = false;
	try {
		context.Request.Body.Position = 0;
	} catch (NotSupportedException) {
		thrown = true;
	}

	var second = await new StreamReader(context.Request.Body, leaveOpen: true).ReadToEndAsync();

	var json = JsonSerializer.Serialize(new { canSeek, first, second, thrown }, JsonHelper.Options);
	await context.Response.WriteAsync(json);
});

// クライアント・サーバのIPアドレス・ポート番号を確認するEndpoint
app.MapGet("/connection", async context => {
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
app.MapGet("contenttype/{subpath}", async context => {
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
app.MapGet("/header", async context => {
	await context.Response.WriteAsync("Header");
});

// jsonを扱うEndpoint
app.MapPost("/json", async context => {
	// ReadFromJsonAsync/WriteAsJsonAsync - .NET 5からの機能
	// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequestjsonextensions?view=aspnetcore-5.0
	// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpresponsejsonextensions?view=aspnetcore-5.0
	// リクエストからJSONを読み込む
	var sample = await context.Request.ReadFromJsonAsync<Sample>(JsonHelper.Options);

	// レスポンスにJSONを書き込む
	await context.Response.WriteAsJsonAsync(sample, JsonHelper.Options);
});

// リクエストを確認するEndpoint
app.MapGet("/request/{**path}", async context => {
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

app.MapGet("/tlshandshake", async context => {
	var feature = context.Features.GetRequiredFeature<ITlsHandshakeFeature>();

	var tlsHandshake = new {
		feature.CipherAlgorithm,
		feature.CipherStrength,
		feature.HashAlgorithm,
		feature.HashStrength,
		feature.HostName,
		feature.KeyExchangeAlgorithm,
		feature.KeyExchangeStrength,
		feature.NegotiatedCipherSuite,
		// SslProtocolsにはFlags属性が設定されているが
		// ITlsHandshakeFeature経由で取得できるのは1つなのかも
		feature.Protocol
	};

	var json = JsonSerializer.Serialize(tlsHandshake, JsonHelper.Options);
	await context.Response.WriteAsync(json);
});

app.MapGet("/", async context => {
	await context.Response.WriteAsync("Hello World!");
});

app.Run();

// MiscWebApp.Testのため
public partial class Program {
}
