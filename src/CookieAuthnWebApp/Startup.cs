using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CookieAuthnWebApp {
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

					options.Events = new LoggingCookieAuthenticationEvents {
						OnSigningIn = (CookieSigningInContext context) => {
							// Claimを追加できる
							var identity = (ClaimsIdentity)context.Principal.Identity;
							identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
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

					// サインイン
					// 認証クッキーをつけたレスポンスを返す
					await context.SignInAsync(principal);
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
					var principal = result.Principal;

					var model = new {
						result.Succeeded,
						NameIdentifier = principal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "",
						Role = principal?.FindFirstValue(ClaimTypes.Role) ?? "",
					};
					var options = new JsonSerializerOptions {
						PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
					};
					var json = JsonSerializer.Serialize(model, options);
					await context.Response.WriteAsync(json);
					// 認証した（~/sigininにリクエストを送った）後だと
					// Succeeded: True
					// NameIdentifier: 1
					// Role: Admin
					// 認証していないと
					// Succeeded: False
					// NameIdentifier:
					// Role:
				});

				endpoints.MapGet("/", async context => {
					await context.Response.WriteAsync("Hello World!");
				});
			});
		}
	}
}
