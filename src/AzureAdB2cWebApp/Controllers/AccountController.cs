using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

		public AccountController(IConfiguration config) {
			_config = config;
		}

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

		// todo:
		/*
		public IActionResult SignOut() {
			return new EmptyResult();
		}

		public IActionResult ResetPassword() {
			return new EmptyResult();
		}

		public IActionResult EditProfile() {
			return new EmptyResult();
		}
		*/
	}
}
