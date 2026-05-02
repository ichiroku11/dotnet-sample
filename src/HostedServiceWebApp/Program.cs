using HostedServiceWebApp.Services;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

// スケジュールに従って定期的な処理を実行するサンプル
services
	.AddHostedService<SampleScheduleService>();

// AddScopedで追加されたサービスをIHostedServiceから呼び出すサンプル
services
	.AddScoped<SampleScopedService>()
	.AddHostedService<SampleBackgroundServiceWithScopedService>();

services
	// BackgroundService継承したサンプル
	.AddHostedService<SampleBackgroundService>()
	// IHostedServiceを実装したサンプル
	.AddHostedService<SampleHostedService>();

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGet("/", async context => {
	await context.Response.WriteAsync("Hello World!");
});

app.Run();
