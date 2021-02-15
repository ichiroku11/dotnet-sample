using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareWebApp {
	public static class SampleMiddlewareExtensions {
		public static IApplicationBuilder UseSample(this IApplicationBuilder builder, string label) {
			builder.UseMiddleware<SampleMiddleware>(label);
			return builder;
		}
	}
}
