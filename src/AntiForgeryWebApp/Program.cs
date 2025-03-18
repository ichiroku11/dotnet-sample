using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

var services = builder.Services;

services.AddControllersWithViews(options => {
	// GET/HEAD/OPTIONS/TRACE以外のメソッドに対してトークンの検証を行う
	options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

services.Configure<RouteOptions>(options => {
	options.LowercaseUrls = true;
	options.LowercaseQueryStrings = true;
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
