using AzureAdB2cApiConnectorWebApp;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapUserCreatingEndpoints();
app.MapGet("/", () => "Hello World!");

app.Run();
