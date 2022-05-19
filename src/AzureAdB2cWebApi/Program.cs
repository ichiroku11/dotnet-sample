using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(
		configuration: config,
		configSectionName: "AzureAdB2c",
		//jwtBearerScheme: "Bearer",
		// OpenIdConnectEventsのデバッグログの出力
		subscribeToJwtBearerMiddlewareDiagnosticsEvents: true);

services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
	endpoints.MapControllerRoute(
		"default",
		"{controller=Default}/{action=Index}/{id?}");
});

app.Run();

// https://docs.microsoft.com/ja-jp/azure/active-directory-b2c/enable-authentication-web-api?tabs=csharpclient
// https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2

