namespace MiddlewareWebApp;

public static class SampleMiddlewareExtensions {
	public static IApplicationBuilder UseSample(this IApplicationBuilder builder, string label) {
		builder.UseMiddleware<SampleMiddleware>(label);
		return builder;
	}
}
