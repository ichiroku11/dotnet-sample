using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApp.Test;

public abstract class TestBase : IClassFixture<WebApplicationFactory<Program>>, IDisposable {
	private readonly WebApplicationFactory<Program> _factory;
	private HttpClient _client;

	protected TestBase(ITestOutputHelper output, WebApplicationFactory<Program> factory) {
		Output = output;
		_factory = factory;
		_client = _factory.CreateClient();
	}

	protected ITestOutputHelper Output { get; }

	public void Dispose() {
		_client.Dispose();
	}

	protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
		Output.WriteLine(request.ToString());
		if (request.Content != null) {
			Output.WriteLine(await request.Content.ReadAsStringAsync());
		}

		var response = await _client.SendAsync(request);

		Output.WriteLine(response.ToString());
		if (response.Content != null) {
			Output.WriteLine(await response.Content.ReadAsStringAsync());
		}
		return response;
	}
}
