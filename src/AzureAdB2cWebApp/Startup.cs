using Microsoft.Identity.Web;

namespace AzureAdB2cWebApp;

public class Startup(IConfiguration config) {
	private readonly IConfiguration _config = config;

	public void ConfigureServices(IServiceCollection services) {
		services.AddControllersWithViews();

		// Azure AD B2C認証
		// Microsoft.Identity.Webを使う
		services.AddMicrosoftIdentityWebAppAuthentication(
			configuration: _config,
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
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		// 認証
		app.UseAuthentication();

		// 承認
		// AuthorizeAttributeのために必要
		app.UseAuthorization();

		app.UseEndpoints(endpoints => {
			endpoints.MapDefaultControllerRoute();
		});
	}
}
