using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public abstract class ControllerTestBase : IClassFixture<WebApplicationFactory<Program>> {
	private readonly ITestOutputHelper _output;
	private readonly WebApplicationFactory<Program> _factory;

	protected ControllerTestBase(ITestOutputHelper output, WebApplicationFactory<Program> factory) {
		_output = output;
		_factory = factory;
	}

	protected void WriteLine(string message) => _output.WriteLine(message);

	protected HttpClient CreateClient(Action<IServiceCollection>? configure = default, bool logging = true) {
		var options = _factory.ClientOptions;

		// 参考
		// https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Testing/src/WebApplicationFactoryClientOptions.cs#L64-L79
		IEnumerable<DelegatingHandler> createHandlers() {
			if (logging) {
				yield return new LoggingHandler(_output);
			}

			if (options.AllowAutoRedirect) {
				yield return new RedirectHandler(options.MaxAutomaticRedirections);
			}

			if (options.HandleCookies) {
				yield return new CookieContainerHandler();
			}
		}

		return _factory
			.WithWebHostBuilder(builder => {
				if (configure is not null) {
					builder.ConfigureTestServices(configure);
				}
			})
			.CreateDefaultClient(options.BaseAddress, createHandlers().ToArray());
	}

	private class LoggingHandler(ITestOutputHelper output) : DelegatingHandler {
		private readonly ITestOutputHelper _output = output;

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			_output.WriteLine(request.ToString());
			if (request.Content != null) {
				_output.WriteLine(await request.Content.ReadAsStringAsync(cancellationToken));
			}

			var response = await base.SendAsync(request, cancellationToken);

			_output.WriteLine(response.ToString());
			if (response.Content != null) {
				_output.WriteLine(await response.Content.ReadAsStringAsync(cancellationToken));
			}
			return response;
		}
	}
}
