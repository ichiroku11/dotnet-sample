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

		// Implements the interface method by invoking the related delegate method.
		public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context) {
			GetLogger(context).LogCallerMethodName();
			return base.RedirectToAccessDenied(context);
		}

		// Implements the interface method by invoking the related delegate method.
		public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context) {
			GetLogger(context).LogCallerMethodName();
			return base.RedirectToLogin(context);
		}

		// Implements the interface method by invoking the related delegate method.
		public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context) {
			GetLogger(context).LogCallerMethodName();
			return base.RedirectToLogout(context);
		}

		// Implements the interface method by invoking the related delegate method.
		public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context) {
			GetLogger(context).LogCallerMethodName();
			return base.RedirectToReturnUrl(context);
		}

		// Implements the interface method by invoking the related delegate method.
		public override Task SignedIn(CookieSignedInContext context) {
			GetLogger(context).LogCallerMethodName();
			return base.SignedIn(context);
		}

		// Implements the interface method by invoking the related delegate method.
		public override Task SigningIn(CookieSigningInContext context) {
			GetLogger(context).LogCallerMethodName();
			return base.SigningIn(context);
		}

		// Implements the interface method by invoking the related delegate method.
		public override Task SigningOut(CookieSigningOutContext context) {
			GetLogger(context).LogCallerMethodName();
			return base.SigningOut(context);
		}

		// Implements the interface method by invoking the related delegate method.
		public override Task ValidatePrincipal(CookieValidatePrincipalContext context) {
			GetLogger(context).LogCallerMethodName();
			return base.ValidatePrincipal(context);
		}
	}
}
