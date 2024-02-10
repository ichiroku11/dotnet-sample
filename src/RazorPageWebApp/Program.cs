using RazorPageWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddRazorPages();

services.AddScoped<MonsterRepository>();

var app = builder.Build();

app.UseRouting();
app.MapRazorPages();

app.Run();
