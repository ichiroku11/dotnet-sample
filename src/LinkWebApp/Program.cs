var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();

services.Configure<RouteOptions>(options => {
	options.LowercaseUrls = true;
	options.LowercaseQueryStrings = true;
});

var app = builder.Build();

app.UseRouting();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();

public partial class Program {
}
