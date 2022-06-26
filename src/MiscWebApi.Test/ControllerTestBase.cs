using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApi;

public abstract class ControllerTestBase : IClassFixture<WebApplicationFactory<Program>> {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};
	protected static StringContent GetJsonStringContent<TModel>(TModel model) {
		var json = JsonSerializer.Serialize(model, _jsonSerializerOptions);
		var content = new StringContent(json);
		return content;
	}

	protected static async Task<TModel?> DeserializeAsync<TModel>(HttpResponseMessage response) {
		var json = await response.Content.ReadAsStringAsync();
		var model = JsonSerializer.Deserialize<TModel>(json, _jsonSerializerOptions);
		return model;
	}

	private readonly ITestOutputHelper _output;
	private readonly WebApplicationFactory<Program> _factory;

	protected ControllerTestBase(ITestOutputHelper output, WebApplicationFactory<Program> factory) {
		_output = output;
		_factory = factory;
	}

	protected void WriteLine(string message) => _output.WriteLine(message);

	protected HttpClient CreateClient() {
		return _factory.CreateDefaultClient(new LoggingHandler(_output));
	}

	private class LoggingHandler : DelegatingHandler {
		private readonly ITestOutputHelper _output;

		public LoggingHandler(ITestOutputHelper output) {
			_output = output;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			_output.WriteLine(request.ToString());
			if (request.Content != null) {
				_output.WriteLine(await request.Content.ReadAsStringAsync());
			}

			var response = await base.SendAsync(request, cancellationToken);

			_output.WriteLine(response.ToString());
			if (response.Content != null) {
				_output.WriteLine(await response.Content.ReadAsStringAsync());
			}
			return response;

		}
	}
}
