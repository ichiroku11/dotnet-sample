using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;

namespace AzureAdB2cWebApp.Controllers;

[Authorize]
public class AccountController(IConfiguration config, IOptions<MicrosoftIdentityOptions> options) : Controller {
	private const string _scheme = OpenIdConnectDefaults.AuthenticationScheme;

	private readonly IConfiguration _config = config;
	private readonly MicrosoftIdentityOptions _options = options.Value;

	private ChallengeResult ChallengeOpenIdConnect(AuthenticationProperties properties) => Challenge(properties, _scheme);

	// SignUpSignIn、SignIn、SignUp、ResetPassword、EditProfileのコードの共通化はあえてしていない
	[AllowAnonymous]
	public IActionResult SignUpSignIn(string? prompt = null) {
		var properties = new AuthenticationProperties {
			RedirectUri = Url.Action("Claim", "Home"),
		};

		properties.SetPromptIfValid(prompt);
		// ポリシーは変更しなくてよいはず
		//properties.SetPolicy(_options.SignUpSignInPolicyId);

		return ChallengeOpenIdConnect(properties);
	}

	[AllowAnonymous]
	public IActionResult SignIn(string? prompt = null) {
		var properties = new AuthenticationProperties {
			RedirectUri = Url.Action("Claim", "Home"),
		};

		properties
			.SetPromptIfValid(prompt)
			.SetPolicy(_config["AzureAdB2c:SignInPolicyId"]);

		return ChallengeOpenIdConnect(properties);
	}

	[AllowAnonymous]
	public IActionResult SignUp() {
		var properties = new AuthenticationProperties {
			RedirectUri = Url.Action("Claim", "Home"),
		};

		properties.SetPolicy(_config["AzureAdB2c:SignUpPolicyId"]);

		return ChallengeOpenIdConnect(properties);
	}

	[AllowAnonymous]
	public IActionResult ResetPassword() {
		var properties = new AuthenticationProperties {
			RedirectUri = Url.Action("Claim", "Home"),
		};

		properties.SetPolicy(_options.ResetPasswordPolicyId);

		return ChallengeOpenIdConnect(properties);
	}

	public IActionResult EditProfile() {
		var properties = new AuthenticationProperties {
			RedirectUri = Url.Action("Claim", "Home"),
		};

		properties.SetPolicy(_options.EditProfilePolicyId);

		return ChallengeOpenIdConnect(properties);
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
		return View();
	}

	[AllowAnonymous]
	public IActionResult Error() {
		return View();
	}
}
