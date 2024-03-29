using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CookieAuthnWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services
			// 認証サービスを追加
			// 戻り値はAuthenticationBuilder
			.AddAuthentication(options => {
				options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
			// クッキー認証ハンドラを追加
			.AddCookie(options => {
				options.Cookie.Name = "auth";

				// リダイレクト用のパスを小文字に
				options.AccessDeniedPath = CookieAuthenticationDefaults.AccessDeniedPath.ToString().ToLower();
				options.LoginPath = CookieAuthenticationDefaults.LoginPath.ToString().ToLower();
				options.LogoutPath = CookieAuthenticationDefaults.LogoutPath.ToString().ToLower();

				// クッキーの有効期限を変更するには、
				// CookieAuthenticationOptions.Cookie.Expirationではなく、
				// CookieAuthenticationOptions.ExpireTimeSpanを指定する
				// https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions.expiretimespan?view=aspnetcore-6.0
				//options.ExpireTimeSpan = TimeSpan.FromDays(1);

				// CookieAuthenticationOptions.Cookie.Expirationは無視されるとのこと
				// Expiration is currently ignored. Use ExpireTimeSpan to control lifetime of cookie authentication.
				// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.cookies.cookieauthenticationoptions.cookie?view=aspnetcore-6.0
				//options.Cookie.Expiration = TimeSpan.FromDays(1);

				options.Events = new LoggingCookieAuthenticationEvents {
					OnSigningIn = (CookieSigningInContext context) => {
						// Claimを追加できる
						var identity = context.Principal?.Identity as ClaimsIdentity;
						identity?.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
						return Task.CompletedTask;
					},
				};
			});

		services.AddScoped<IClaimsTransformation, LoggingClaimsTransformation>();

		// URL、クエリ文字列を小文字
		/*
		services.Configure<RouteOptions>(options => {
			options.LowercaseQueryStrings = true;
			options.LowercaseUrls = true;
		});
		*/
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints => {
			// 参考
			// https://qiita.com/masakura/items/85c59e60cac7f0638c1b

			endpoints.MapGet("/challenge", async context => {
				// ログインURL（CookieAuthenticationOptions.LoginPath）へのリダイレクト（302）を返す
				// 401 Unauthorizedを返すようなイメージだと思う
				await context.ChallengeAsync();
			});

			endpoints.MapGet("/forbid", async context => {
				// アクセス禁止URL（CookieAuthenticationOptions.AccessDeniedPath）へのリダイレクト（302）を返す
				// 403 Forbiddenを返すようなイメージだと思う
				await context.ForbidAsync();
			});

			endpoints.MapGet("/signin", async context => {
				// とあるユーザでログインしたとする

				// プリンシパルを作成
				// authenticationTypeを指定しないとIsAuthenticatedがfalseになり、SignInAsyncで例外が発生する
				// https://docs.microsoft.com/ja-jp/dotnet/core/compatibility/aspnetcore#identity-signinasync-throws-exception-for-unauthenticated-identity
				var identity = new ClaimsIdentity(authenticationType: "Test");
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
				var principal = new ClaimsPrincipal(identity);

				var properties = new AuthenticationProperties(
					items: new Dictionary<string, string?> {
						["x"] = "2",
					},
					parameters: new Dictionary<string, object?>() {
						["y"] = 3,
					});

				// サインイン
				// 認証クッキーをつけたレスポンスを返す
				await context.SignInAsync(principal, properties);
				// Set-Cookie: auth=***;
			});

			endpoints.MapGet("/signout", async context => {
				// サインアウト
				// 認証クッキーをクリアするレスポンスを返す
				await context.SignOutAsync();
				// Set-Cookie: auth=; expires=Thu, 01 Jan 1970 00:00:00 GMT;
			});

			endpoints.MapGet("/authenticate", async context => {
				// 認証クッキーからプリンシパルを作成する
				// UseAuthentication/AuthenticationMiddlewareでやってくれてること

				var result = await context.AuthenticateAsync();
				var model = default(object);
				if (result.Succeeded) {
					var principal = result.Principal;
					var properties = result.Properties;
					model = new {
						result.Succeeded,
						NameIdentifier = principal.FindFirstValue(ClaimTypes.NameIdentifier),
						Role = principal.FindFirstValue(ClaimTypes.Role),
						// Itemsは永続化される（クッキーにシリアライズされる）
						Item = properties.Items.TryGetValue("x", out var x) ? x : "",
						// Parametersは永続化されない
						Parameter = properties.Parameters.TryGetValue("y", out var y) ? y : "",
					};
				} else {
					model = new {
						result.Succeeded,
						NameIdentifier = "",
						Role = "",
						Item = "",
						Parameter = "",
					};
				}
				var options = new JsonSerializerOptions {
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				};
				var json = JsonSerializer.Serialize(model, options);
				await context.Response.WriteAsync(json);
				// 認証した（~/sigininにリクエストを送った）後だと
				// Succeeded: True
				// NameIdentifier: 1
				// Role: Admin
				// Item: 1
				// Parameter: 
			});

			endpoints.MapGet("/", async context => {
				await context.Response.WriteAsync("Hello World!");
			});
		});
	}
}
