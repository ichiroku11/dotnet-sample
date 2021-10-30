using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookieAuthnWebApp {
	public class LoggingCookieAuthenticationEvents : CookieAuthenticationEvents {
		private ILogger _logger;

		private ILogger GetLogger<TOptions>(BaseContext<TOptions> context) where TOptions : AuthenticationSchemeOptions {
			if (_logger == null) {
				_logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<LoggingCookieAuthenticationEvents>>();
			}
			return _logger;
		}

		public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context) {
			GetLogger(context).LogCallerMethodName();
			return base.RedirectToAccessDenied(context);
		}
		public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context) {
			GetLogger(context).LogCallerMethodName();
			return base.RedirectToLogin(context);
		}

		public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context) {
			GetLogger(context).LogCallerMethodName();
			return base.RedirectToLogout(context);
		}

		public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context) {
			GetLogger(context).LogCallerMethodName();
			return base.RedirectToReturnUrl(context);
		}

		public override Task SignedIn(CookieSignedInContext context) {
			GetLogger(context).LogCallerMethodName();
			return base.SignedIn(context);
		}

		public override Task SigningIn(CookieSigningInContext context) {
			var logger = GetLogger(context);
			logger.LogCallerMethodName();

			// SignInAsyncメソッドの引数に指定したAuthenticationPropertiesのItemsとParametersを取得できる
			var properties = context.Properties;
			foreach (var item in properties.Items) {
				logger.LogInformation($"items: {item.Key} = {item.Value}");
			}
			foreach (var parameter in properties.Parameters) {
				logger.LogInformation($"items: {parameter.Key} = {parameter.Value}");
			}

			return base.SigningIn(context);
		}

		public override Task SigningOut(CookieSigningOutContext context) {
			GetLogger(context).LogCallerMethodName();
			return base.SigningOut(context);
		}

		public override Task ValidatePrincipal(CookieValidatePrincipalContext context) {
			GetLogger(context).LogCallerMethodName();
			return base.ValidatePrincipal(context);
		}
	}
}
