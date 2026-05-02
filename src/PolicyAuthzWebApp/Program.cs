using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using PolicyAuthzWebApp;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;
var services = builder.Services;

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

app.MapAreaControllerRoute(
		name: "admin",
		areaName: "Admin",
		pattern: "Admin/{controller=Default}/{action=Index}/{id?}")
	.RequireAuthorization(new AuthorizeAttribute(PolicyNames.AdminRole));

app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Default}/{action=Index}/{id?}")
	// コントローラやアクションにAuthorizeAttributeを指定することをほぼ同じことだと思う
	.RequireAuthorization(new AuthorizeAttribute(PolicyNames.Authenticated));

app.Run();
