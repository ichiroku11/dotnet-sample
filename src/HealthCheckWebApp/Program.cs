using HealthCheckWebApp;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// 参考
// https://learn.microsoft.com/ja-jp/aspnet/core/host-and-deploy/health-checks

// AddCheckでCheckを追加しない場合はHealthyだった
services.AddHealthChecks()
	.AddCheck<AlwaysHealthyHealthCheck>("AlwaysHealthy")
	.AddCheck<AlwaysUnhealthyHealthCheck>("AlwaysUnhealthy");

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// 複数のヘルスチェックがある場合、最も悪いステータスになる
app.MapHealthChecks("/health");

app.Run();
