var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

services.AddControllersWithViews();

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
