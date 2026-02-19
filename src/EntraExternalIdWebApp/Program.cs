using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

var services = builder.Services;
services
	.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApp(
		configureMicrosoftIdentityOptions: options => {
			config.GetSection("AzureAd").Bind(options);
		},
		configureCookieAuthenticationOptions: options => {

		},
		subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);

services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/claim", static async (ClaimsPrincipal user) => {
	var claims = user.Claims.Select(claim => new { claim.Type, claim.Value });
	return TypedResults.Json(claims);
})
	.RequireAuthorization();

app.MapGet("/account/signin", static () => {
	var authProps = new AuthenticationProperties {
		RedirectUri = "/claim",
	};

	return TypedResults.Challenge(authProps, [OpenIdConnectDefaults.AuthenticationScheme]);
});

app.MapGet("/", () => "Hello World!");

app.Run();
