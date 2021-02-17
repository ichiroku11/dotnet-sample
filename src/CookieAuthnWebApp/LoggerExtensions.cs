using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CookieAuthnWebApp {
	public static class LoggerExtensions {
		public static void LogCallerMethodName(this ILogger logger, [CallerMemberName]string caller = null) {
			logger.LogInformation(caller);
		}
	}
}
