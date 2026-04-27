var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var env = builder.Environment;

services.AddControllersWithViews();

services.Configure<RouteOptions>(options => {
	options.LowercaseQueryStrings = true;
	options.LowercaseUrls = true;
});

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapControllerRoute(
	name: "default",
pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
