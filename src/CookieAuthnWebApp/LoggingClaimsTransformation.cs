using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CookieAuthnWebApp;

public class LoggingClaimsTransformation(ILogger<LoggingClaimsTransformation> logger) : IClaimsTransformation {
	private readonly ILogger _logger = logger;

	public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal) {
		_logger.LogInformation(nameof(TransformAsync));

		return Task.FromResult(principal);
	}
}
