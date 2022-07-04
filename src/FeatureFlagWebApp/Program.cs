using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddFeatureManagement();
services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => {
	endpoints.MapDefaultControllerRoute();
});

app.Run();
