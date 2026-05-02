using OptionPatternWebApp;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;
var services = builder.Services;

// 設定をクラスにバインドできるようにする
services.Configure<SampleOptions>(config.GetSection("App:Sample"));
// 別の方法
//services.Configure<SampleOptions>(_config.GetSection("App").GetSection("Sample"));
services.Configure<SampleOptionsMonitor>(config.GetSection("App:SampleMonitor"));

services.AddControllers();

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
