using Microsoft.AspNetCore.Authentication;

namespace BasicAuthnWebApp;

public class BasicAuthenticationOptions : AuthenticationSchemeOptions {
	public ICredentialsValidator? CredentialsValidator { get; set; }
}
