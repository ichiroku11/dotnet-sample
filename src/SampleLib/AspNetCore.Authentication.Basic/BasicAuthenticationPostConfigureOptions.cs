using Microsoft.Extensions.Options;

namespace SampleLib.AspNetCore.Authentication.Basic;

public class BasicAuthenticationPostConfigureOptions : IPostConfigureOptions<BasicAuthenticationOptions> {
	public void PostConfigure(string? name, BasicAuthenticationOptions options) {
	}
}
