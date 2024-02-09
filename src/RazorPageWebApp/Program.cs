var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddRazorPages();

var app = builder.Build();

app.UseRouting();
app.MapRazorPages();

app.Run();
