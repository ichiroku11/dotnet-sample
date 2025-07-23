
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services
	.AddDistributedMemoryCache(options => {
	})
	.AddSession(options => {
		options.Cookie.Name = "session";
	});

var app = builder.Build();

app.UseRouting();

app.UseSession();

// セッション一覧
app.MapGet("/session", async context => {
	var session = context.Session;
	foreach (var key in session.Keys) {
		await context.Response.WriteAsync($"{key}: {session.GetString(key)}");
		await context.Response.WriteAsync(Environment.NewLine);
	}
});

// セッションに設定
app.MapGet("/session/set/{key}:{value}", async context => {
	var session = context.Session;

	var key = context.Request.RouteValues["key"] as string;
	var value = context.Request.RouteValues["value"] as string;
	if (key is null || value is null) {
		context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		return;
	}

	session.SetString(key, value);

	await context.Response.WriteAsync($"{key}: {session.GetString(key)}");
});

// セッションから削除
app.MapGet("/session/remove/{key}", async context => {
	var session = context.Session;

	var key = context.Request.RouteValues["key"] as string;
	if (key is null) {
		context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		return;
	}

	session.Remove(key);

	await context.Response.WriteAsync($"{key}:");
});

app.MapGet("/", async context => {
	await context.Response.WriteAsync("Hello World!");
});

app.Run();
