using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddFeatureManagement();
services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
