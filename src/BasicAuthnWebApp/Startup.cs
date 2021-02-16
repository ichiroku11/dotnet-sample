using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BasicAuthnWebApp {
	public class Startup {
		// テスト用
		private class TestCredentialsValidator : ICredentialsValidator {
			public Task<ClaimsPrincipal> ValidateAsync(string userName, string password, AuthenticationScheme scheme) {
				// 仮に
				if (!string.Equals(userName, "abc", StringComparison.Ordinal) ||
					!string.Equals(password, "xyz", StringComparison.Ordinal)) {
					return null;
				}

				var claims = new[] {
					new Claim(ClaimTypes.NameIdentifier, userName),
				};
				var identity = new ClaimsIdentity(claims, scheme.Name);
				var principal = new ClaimsPrincipal(identity);

				return Task.FromResult(principal);
			}
		}

		public void ConfigureServices(IServiceCollection services) {
			// 認証
			services
				.AddAuthentication(options => {
					options.DefaultScheme = BasicAuthenticationDefaults.AuthenticationScheme;
				})
				// Basic認証ハンドラ
				.AddBasic(options => {
					options.CredentialsValidator = new TestCredentialsValidator();
				});

			// 承認
			services
				.AddAuthorization(options => {
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
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{action}/{id?}",
					defaults: new { controller = "Default" });
			});
		}
	}
}
