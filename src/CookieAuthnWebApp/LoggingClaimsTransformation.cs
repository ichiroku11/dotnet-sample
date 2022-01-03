using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CookieAuthnWebApp;

public class LoggingClaimsTransformation : IClaimsTransformation {
	private readonly ILogger _logger;

	public LoggingClaimsTransformation(ILogger<LoggingClaimsTransformation> logger) {
		_logger = logger;
	}

	public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal) {
		_logger.LogInformation(nameof(TransformAsync));

		return Task.FromResult(principal);
	}
}
