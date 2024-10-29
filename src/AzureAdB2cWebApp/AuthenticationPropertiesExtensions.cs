using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AzureAdB2cWebApp;

public static class AuthenticationPropertiesExtensions {
	public static AuthenticationProperties SetPromptIfValid(this AuthenticationProperties properties, string? prompt) {
		if (!OpenIdConnectPromptValidator.IsValid(prompt)) {
			return properties;
		}

		properties.SetParameter(OpenIdConnectParameterNames.Prompt, prompt);

		return properties;
	}

	private const string _policy = "policy";

	public static AuthenticationProperties SetPolicy(this AuthenticationProperties properties, string? policy) {
		properties.Items[_policy] = policy;

		return properties;
	}
}
