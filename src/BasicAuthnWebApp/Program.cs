using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;
using SampleLib.AspNetCore.Authentication.Basic;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// 認証
services
	.AddAuthentication(options => {
		options.DefaultScheme = BasicAuthenticationDefaults.AuthenticationScheme;
	})
	// Basic認証ハンドラ
	.AddBasic(options => {
		// todo:
		options.CredentialsValidator = new TestCredentialsValidator();
	});

// 承認
services.AddAuthorization(options => {
	// 認証ポリシー
	options.AddPolicy("Authenticated", builder => {
		builder.RequireAuthenticatedUser();
	});
});

// MVC（コントローラのみ）
services.AddControllers(options => {
	// グローバルフィルタで認証必須に
	options.Filters.Add(new AuthorizeFilter("Authenticated"));
});

services.Configure<RouteOptions>(options => {
	options.LowercaseQueryStrings = true;
	options.LowercaseUrls = true;
});

var app = builder.Build();
var env = app.Environment;

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{action}/{id?}",
	defaults: new { controller = "Default" });

app.Run();

// テスト用
public class TestCredentialsValidator : ICredentialsValidator {
	public Task<ClaimsPrincipal?> ValidateAsync(string userName, string password, AuthenticationScheme scheme) {
		// 仮に
		if (!string.Equals(userName, "abc", StringComparison.Ordinal) ||
			!string.Equals(password, "xyz", StringComparison.Ordinal)) {
			return Task.FromResult<ClaimsPrincipal?>(null);
		}

		var claims = new[] {
			new Claim(ClaimTypes.NameIdentifier, userName),
		};
		var identity = new ClaimsIdentity(claims, scheme.Name);
		var principal = new ClaimsPrincipal(identity);

		return Task.FromResult<ClaimsPrincipal?>(principal);
	}
}

// ユニットテストのため
public partial class Program {
}
