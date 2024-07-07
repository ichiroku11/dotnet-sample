using Microsoft.AspNetCore.Authentication;

namespace SampleLib.AspNetCore.Authentication.Basic;

public class BasicAuthenticationOptions : AuthenticationSchemeOptions {
	public ICredentialsValidator? CredentialsValidator { get; set; }
}
