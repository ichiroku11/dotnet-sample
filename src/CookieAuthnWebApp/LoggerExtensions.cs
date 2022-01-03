using System.Runtime.CompilerServices;

namespace CookieAuthnWebApp;

public static class LoggerExtensions {
	public static void LogCallerMethodName(this ILogger logger, [CallerMemberName] string? caller = null) {
		logger.LogInformation(caller);
	}
}
