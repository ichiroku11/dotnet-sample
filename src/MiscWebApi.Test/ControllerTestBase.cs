using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApi {
	public abstract class ControllerTestBase : IClassFixture<WebApplicationFactory<Startup>>, IDisposable {
		private static readonly JsonSerializerOptions _jsonSerializerOptions
			= new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true,
			};
		protected static StringContent GetJsonStringContent<TModel>(TModel model) {
			var json = JsonSerializer.Serialize(model, _jsonSerializerOptions);
			var content = new StringContent(json);
			return content;
		}

		protected static async Task<TModel> DeserializeAsync<TModel>(HttpResponseMessage response) {
			var json = await response.Content.ReadAsStringAsync();
			var model = JsonSerializer.Deserialize<TModel>(json, _jsonSerializerOptions);
			return model;
		}

		private readonly ITestOutputHelper _output;
		private readonly WebApplicationFactory<Startup> _factory;
		private HttpClient _client;

		protected ControllerTestBase(ITestOutputHelper output, WebApplicationFactory<Startup> factory) {
			_output = output;
			_factory = factory;
			_client = _factory.CreateClient();
		}

		public void Dispose() {
			_client?.Dispose();
			_client = null;
		}

		protected void WriteLine(string message) => _output.WriteLine(message);

		protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
			_output.WriteLine(request.ToString());
			if (request.Content != null) {
				_output.WriteLine(await request.Content.ReadAsStringAsync());
			}

			var response = await _client.SendAsync(request);

			_output.WriteLine(response.ToString());
			if (response.Content != null) {
				_output.WriteLine(await response.Content.ReadAsStringAsync());
			}
			return response;
		}
	}
}
