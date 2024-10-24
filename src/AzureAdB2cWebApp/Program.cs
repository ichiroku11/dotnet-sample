using AzureAdB2cWebApp;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

var services = builder.Services;

services.AddControllersWithViews();

// Azure AD B2C認証
// Microsoft.Identity.Webを使う
services.AddMicrosoftIdentityWebAppAuthentication(
	configuration: config,
	configSectionName: "AzureAdB2c",
	// OpenIdConnectEventsのデバッグログの出力
	subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);

var handlers = new OpenIdConnectEventHandlers();
services.PostConfigure<MicrosoftIdentityOptions>(options => {
	// Microsoft.Identity.Web.UIを使わず動きを確認したいため、
	// デフォルトの動きを上書きする
	// https://github.com/AzureAD/microsoft-identity-web/blob/master/src/Microsoft.Identity.Web/AzureADB2COpenIDConnectEventHandlers.cs
	options.Events.OnRemoteFailure = handlers.OnRemoteFailure;
});

services.Configure<RouteOptions>(options => {
	options.LowercaseUrls = true;
	options.LowercaseQueryStrings = true;
});

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

// 認証
app.UseAuthentication();

// 承認
// AuthorizeAttributeのために必要
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
