
using Microsoft.Extensions.Options;
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

// セッションID
app.MapGet("/session/id", async context => {
	await context.Response.WriteAsync(context.Session.Id);
});

// セッションをクリア
app.MapGet("/session/clear", context => {
	// セッションクッキーは削除されない
	context.Session.Clear();

	return Task.CompletedTask;
});

// セッションクッキーを削除
app.MapGet("/session/cookie/delete", context => {
	// セッションクッキーは削除される

	var options = context.RequestServices.GetRequiredService<IOptions<SessionOptions>>();
	var key = options.Value.Cookie.Name ?? throw new InvalidOperationException();
	context.Response.Cookies.Delete(key);
	// SetCookieが設定される

	return Task.CompletedTask;
});

// セッションに設定
app.MapGet("/session/set/{key}:{value}", async (HttpContext context, string? key, string? value) => {
	var session = context.Session;

	if (key is null || value is null) {
		context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		return;
	}

	session.SetString(key, value);

	await context.Response.WriteAsync($"{key}: {session.GetString(key)}");
});

// セッションから削除
app.MapGet("/session/remove/{key}", async (HttpContext context, string? key) => {
	var session = context.Session;

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
