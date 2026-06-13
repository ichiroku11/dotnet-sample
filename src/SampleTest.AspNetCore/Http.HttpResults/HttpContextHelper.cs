using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace SampleTest.AspNetCore.Http.HttpResults;

public class HttpContextHelper {
	public static HttpContext CreateWithResponseBody() {
		var services = new ServiceCollection();
		services.AddSingleton<ILoggerFactory, NullLoggerFactory>();

		var context = new DefaultHttpContext {
			RequestServices = services.BuildServiceProvider(),
		};
		context.Response.Body = new MemoryStream();

		return context;
	}
}
