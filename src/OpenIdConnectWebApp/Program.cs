using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;

// 参考
// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/samples/MinimalOpenIdConnectSample/Program.cs
// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/samples/OpenIdConnectSample/Startup.cs

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services
	.AddAuthentication(options => {
		options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
	})
	.AddCookie(options => {
		// todo:
	})
	.AddOpenIdConnect(options => {
		config.GetSection("OpenIdConnect").Bind(options);

		// 下記より
		// https://developers.line.biz/ja/docs/line-login/verify-id-token/
		// OpenIDプロバイダーの情報
		// todo: LineDefaultsか
		options.MetadataAddress = "https://access.line.me/.well-known/openid-configuration";
	});

services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGet("/protected", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}!")
	.RequireAuthorization();

app.Run();
