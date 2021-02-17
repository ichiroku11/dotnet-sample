using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookieAuthnWebApp {
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
}
