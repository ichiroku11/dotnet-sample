using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;

// 参考
// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/samples/MinimalOpenIdConnectSample/Program.cs
// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/samples/OpenIdConnectSample/Startup.cs

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services
	.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
	.AddCookie(options => {
		// todo:
	})
	.AddOpenIdConnect(options => {
		// todo:
	});

services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();

app.MapGet("/", () => "Hello World!");
app.MapGet("/protected", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}!")
	.RequireAuthorization();

app.Run();
