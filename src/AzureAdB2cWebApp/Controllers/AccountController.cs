using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAdB2cWebApp.Controllers {
	[Authorize]
	public class AccountController : Controller {
		private const string _scheme = OpenIdConnectDefaults.AuthenticationScheme;
		private const string _policy = "policy";

		private readonly IConfiguration _config;
		private readonly MicrosoftIdentityOptions _options;

		public AccountController(IConfiguration config, IOptions<MicrosoftIdentityOptions> options) {
			_config = config;
			_options = options.Value;
		}

		// SignIn、SignUp、ResetPassword、EditProfileのコードの共通化はあえてしていない
		[AllowAnonymous]
		public IActionResult SignIn() {
			var properties = new AuthenticationProperties {
				RedirectUri = Url.Action("Claim", "Home"),
			};
			properties.Items[_policy] = _config["AzureAdB2c:SignInPolicyId"];

			return Challenge(properties, _scheme);
		}

		[AllowAnonymous]
		public IActionResult SignUp() {
			var properties = new AuthenticationProperties {
				RedirectUri = Url.Action("Claim", "Home"),
			};
			properties.Items[_policy] = _config["AzureAdB2c:SignUpPolicyId"];

			return Challenge(properties, _scheme);
		}

		public IActionResult ResetPassword() {
			var properties = new AuthenticationProperties {
				RedirectUri = Url.Action("Claim", "Home"),
			};
			properties.Items[_policy] = _options.ResetPasswordPolicyId;

			return Challenge(properties, _scheme);
		}

		public IActionResult EditProfile() {
			var properties = new AuthenticationProperties {
				RedirectUri = Url.Action("Claim", "Home"),
			};
			properties.Items[_policy] = _options.EditProfilePolicyId;

			return Challenge(properties, _scheme);
		}

		// 親クラスでSignOutメソッドが定義されているため
		// しかたなくAsyncメソッドに
		public Task<IActionResult> SignOutAsync() {
			var properties = new AuthenticationProperties {
				RedirectUri = Url.Action("SignedOut"),
			};
			var result = SignOut(
				properties,
				CookieAuthenticationDefaults.AuthenticationScheme,
				OpenIdConnectDefaults.AuthenticationScheme);

			return Task.FromResult<IActionResult>(result);
		}

		[AllowAnonymous]
		public IActionResult SignedOut() {
			return View("Message", "Signed out");
		}

		[AllowAnonymous]
		public IActionResult Error() {
			return View("Message", "Error");
		}
	}
}
