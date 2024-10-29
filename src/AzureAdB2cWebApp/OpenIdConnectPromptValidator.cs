using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AzureAdB2cWebApp;

// 3.1.2.1.  Authentication Request
// https://openid.net/specs/openid-connect-core-1_0.html#AuthRequest
public static class OpenIdConnectPromptValidator {
	public static bool IsValid(string? prompt) {
		// Azure AD B2Cでは"login"と"none"に対応している
		// "login"はドキュメントに記載があるが、"none"については記載なさげ
		// https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/openid-connect
		return
			string.Equals(prompt, OpenIdConnectPrompt.Login, StringComparison.Ordinal) ||
			string.Equals(prompt, OpenIdConnectPrompt.None, StringComparison.Ordinal);
	}
}
