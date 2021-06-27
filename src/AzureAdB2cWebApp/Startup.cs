using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAdB2cWebApp {
	public class Startup {
		private readonly IConfiguration _config;

		public Startup(IConfiguration config) {
			_config = config;
		}

		public void ConfigureServices(IServiceCollection services) {
			services.AddControllersWithViews();

			// Azure AD B2C認証
			// Microsoft.Identity.Webを使う
			services.AddMicrosoftIdentityWebAppAuthentication(
				configuration: _config,
				configSectionName: "AzureAdB2c",
				// OpenIdConnectEventsのデバッグログの出力
				subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);

			var handlers  = new OpenIdConnectEventHandlers();
			services.PostConfigure<MicrosoftIdentityOptions>(options => {
				// Microsoft.Identity.Web.UIを使わず動きを確認したいため、
				// デフォルトの動きを上書きする
				// https://github.com/AzureAD/microsoft-identity-web/blob/master/src/Microsoft.Identity.Web/AzureADB2COpenIDConnectEventHandlers.cs
				var hanlder = options.Events.OnRemoteFailure;
				options.Events.OnRemoteFailure = async context => {
					await handlers.OnRemoteFailure(context);
					await hanlder(context);
				};
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
}
