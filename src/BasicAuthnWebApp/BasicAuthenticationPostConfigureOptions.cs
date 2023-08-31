using Microsoft.Extensions.Options;

namespace BasicAuthnWebApp;

public class BasicAuthenticationPostConfigureOptions : IPostConfigureOptions<BasicAuthenticationOptions> {
	public void PostConfigure(string? name, BasicAuthenticationOptions options) {
	}
}
