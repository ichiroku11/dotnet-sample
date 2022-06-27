using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer();
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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
	endpoints.MapControllerRoute(
		"default",
		"{controller=Default}/{action=Index}/{id?}");
});


app.Run();

// MiscWebApi.Testのため
public partial class Program {
}
