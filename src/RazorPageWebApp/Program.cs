using RazorPageWebApp.Models.Monsters;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddRazorPages();

services.Configure<RouteOptions>(options => {
	options.LowercaseQueryStrings = true;
	options.LowercaseUrls = true;
});

services.AddScoped<MonsterRepository>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
