using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

var services = builder.Services;

services.AddControllersWithViews();

// コードではErrorPathを変更できないのか？
// 下記ではうまくいかず
/*
services.Configure<MicrosoftIdentityOptions>(options => {
//services.PostConfigure<MicrosoftIdentityOptions>(options => {
	options.ErrorPath = "/account/error";
});
*/

// Azure AD B2C認証
// Microsoft.Identity.Webを使う
services.AddMicrosoftIdentityWebAppAuthentication(
	configuration: config,
	configSectionName: "AzureAdB2c",
	// OpenIdConnectEventsのデバッグログの出力
	subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);

// OnRemoteFailureを上書きできない様子なので、別の方法を考える
/*
var handlers = new OpenIdConnectEventHandlers();
services.PostConfigure<MicrosoftIdentityOptions>(options => {
//services.Configure<MicrosoftIdentityOptions>(options => {
//services.PostConfigure<OpenIdConnectOptions>(options => {
//services.Configure<OpenIdConnectOptions>(options => {
	// Microsoft.Identity.Web.UIを使わず動きを確認したいため、
	// デフォルトの動きを上書きする
	// https://github.com/AzureAD/microsoft-identity-web/blob/master/src/Microsoft.Identity.Web/AzureADB2COpenIDConnectEventHandlers.cs
	options.Events.OnRemoteFailure = handlers.OnRemoteFailure;
});
*/

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
