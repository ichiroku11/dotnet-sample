using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Encodings.Web;

namespace SampleLib.AspNetCore.Authentication.Basic;

// 参考
// https://github.com/blowdart/idunno.Authentication/
public class BasicAuthenticationHandler(
	IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
	: AuthenticationHandler<BasicAuthenticationOptions>(options, logger, encoder) {
	protected new BasicAuthenticationEvents? Events {
		get => base.Events as BasicAuthenticationEvents;
		set => base.Events = value;
	}

	protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new BasicAuthenticationEvents());

	protected async override Task<AuthenticateResult> HandleAuthenticateAsync() {
		var headerValue = (string?)Request.Headers[HeaderNames.Authorization];
		if (string.IsNullOrEmpty(headerValue)) {
			return AuthenticateResult.NoResult();
		}

		if (!headerValue.StartsWith("Basic ")) {
			return AuthenticateResult.NoResult();
		}

		var encodedCredentials = headerValue["Basic ".Length..].Trim();
		if (string.IsNullOrEmpty(encodedCredentials)) {
			return AuthenticateResult.Fail("Missing credentials");
		}

		var decoder = new BasicAuthenticationCredentialsDecoder();
		if (!decoder.TryDecode(encodedCredentials, out var userName, out var password)) {
			return AuthenticateResult.Fail("Invalid credentials");
		}

		var principal = Options.CredentialsValidator is null
			? throw new InvalidOperationException()
			: await Options.CredentialsValidator.ValidateAsync(userName, password, Scheme);
		if (principal == null) {
			return AuthenticateResult.Fail("Invalid username or password");
		}

		var ticket = new AuthenticationTicket(principal, Scheme.Name);
		return AuthenticateResult.Success(ticket);
	}

	protected override Task HandleChallengeAsync(AuthenticationProperties properties) {
		// 本当ならこうかな？
		/*
		Response.StatusCode = (int)HttpStatusCode.Unauthorized;
		Response.Headers.Add(HeaderNames.WWWAuthenticate, "Basic");
		*/

		return base.HandleChallengeAsync(properties);
	}
}
