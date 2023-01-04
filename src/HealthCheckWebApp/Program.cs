var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddHealthChecks();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHealthChecks("/health");

app.Run();
