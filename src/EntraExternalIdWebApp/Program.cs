using Microsoft.AspNetCore.Authentication;
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

// サインインへ
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

app.MapGet("/", () => "Hello World!");

app.Run();
