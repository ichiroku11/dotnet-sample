using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OpenIdConnectWebApp.Line;
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
		// todo: "Line"
		options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
	})
	.AddCookie(options => {
	})
	.AddOpenIdConnect(options => {
		config.GetSection("Line").Bind(options);

		options.MetadataAddress = LineDefaults.MetadataAddress;

		// response_typeはcode
		// https://developers.line.biz/ja/docs/line-login/integrate-line-login/#applying-for-email-permission
		options.ResponseType = OpenIdConnectResponseType.Code;

		options.TokenValidationParameters.IssuerSigningKey = options.CreateIssuerSigningKey();
	});

services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGet("/protected", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}!")
	.RequireAuthorization();

app.Run();
