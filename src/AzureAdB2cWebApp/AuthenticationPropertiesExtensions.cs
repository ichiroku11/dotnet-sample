using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AzureAdB2cWebApp;

public static class AuthenticationPropertiesExtensions {
	public static AuthenticationProperties SetPromptIfValid(this AuthenticationProperties properties, string prompt) {
		if (!OpenIdConnectPromptValidator.IsValid(prompt)) {
			return properties;
		}

		properties.SetParameter(OpenIdConnectParameterNames.Prompt, prompt);

		return properties;
	}
}
