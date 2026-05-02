using ViewComponentWebApp.Models;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

services.AddControllersWithViews();

services.AddScoped<TodoRepository>();

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
