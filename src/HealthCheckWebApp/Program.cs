using HealthCheckWebApp;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// 参考
// https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks

// AddCheckでCheckを追加しない場合はHealthyだった
services.AddHealthChecks()
	.AddCheck<AlwaysHealthyHealthCheck>("AlwaysHealthy");

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHealthChecks("/health");

app.Run();
