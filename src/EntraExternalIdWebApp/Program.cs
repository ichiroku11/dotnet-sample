using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

var services = builder.Services;
services
	.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApp(
		configureMicrosoftIdentityOptions: options => {
			// AuthorityとClientIdがあればEntra External IDに接続できる様子
			//config.GetSection("AzureAd").Bind(options);
			options.Authority = config.GetSection("AzureAd:Authority").Value;
			options.ClientId = config.GetSection("AzureAd:ClientId").Value;

			options.AccessDeniedPath = "/account/accessdenied";
			options.ErrorPath = "/account/error";
		},
		configureCookieAuthenticationOptions: options => {

		},
		subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);
services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// 認証後にクレームを表示するエンドポイント
app.MapGet("/claim", static async (ClaimsPrincipal user) => {
	var claims = user.Claims.Select(claim => new { claim.Type, claim.Value });
	return TypedResults.Json(claims);
})
	.RequireAuthorization();

// サインイン
app.MapGet("/account/signin", static (string? prompt) => {
	var authProps = new AuthenticationProperties {
		RedirectUri = "/claim",
	};

	if (!string.IsNullOrWhiteSpace(prompt) && IsValidPrompt(prompt)) {
		authProps.SetParameter(OpenIdConnectParameterNames.Prompt, OpenIdConnectPrompt.Create);
	}

	return TypedResults.Challenge(authProps, [OpenIdConnectDefaults.AuthenticationScheme]);

	static bool IsValidPrompt(string prompt)
		=> prompt is OpenIdConnectPrompt.Create
			or OpenIdConnectPrompt.Consent
			or OpenIdConnectPrompt.Login
			or OpenIdConnectPrompt.None
			or OpenIdConnectPrompt.SelectAccount;
});

// サインアウト
app.MapGet("/account/signout", static () => {
	var authProps = new AuthenticationProperties {
		RedirectUri = "/account/signedout",
	};

	return TypedResults.SignOut(
		authProps,
		[CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]);
});
app.MapGet("/account/signedout", static () => "signedout");

app.MapGet("/account/accessdenied", static () => "accessdenied");
app.MapGet("/account/error", static () => "error");

app.MapGet("/", () => "Hello World!");

app.Run();
