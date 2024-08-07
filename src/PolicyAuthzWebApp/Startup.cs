using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace PolicyAuthzWebApp;

public static class PolicyNames {
	public const string Authenticated = nameof(Authenticated);
	public const string AdminRole = nameof(AdminRole);
}

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		// 認証
		services
			.AddAuthentication(options => {
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			}).AddCookie(options => {
			});

		// 承認
		services
			.AddAuthorizationBuilder()
			.AddPolicy(PolicyNames.Authenticated, builder => {
				builder.RequireAuthenticatedUser();
			})
			.AddPolicy(PolicyNames.AdminRole, builder => {
				builder.RequireRole("admin");
			});
		// todo:
		/*
			.AddPolicy("AreaRoles", builder => {
				builder.RequireAreaRoles(area: "Admin", roles: "admin");
			});
		*/

		// MVC
		services.AddControllers(options => {
			// 承認（Authorization）は
			// グローバルフィルタに指定するか
			// コントローラにAuthorizeAttributeを指定するか
			// エンドポイントにRequireAuthorizationするか
			/*
			options.Filters.Add(new AuthorizeFilter("Authenticated"));
			*/
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
			endpoints
				.MapAreaControllerRoute(
					name: "admin",
					areaName: "Admin",
					pattern: "Admin/{controller=Default}/{action=Index}/{id?}")
				.RequireAuthorization(new AuthorizeAttribute(PolicyNames.AdminRole));

			endpoints
				.MapControllerRoute(
					name: "default",
					pattern: "{controller=Default}/{action=Index}/{id?}")
				// コントローラやアクションにAuthorizeAttributeを指定することをほぼ同じことだと思う
				.RequireAuthorization(new AuthorizeAttribute(PolicyNames.Authenticated));
		});
	}
}
