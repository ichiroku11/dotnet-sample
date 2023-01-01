using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace LinkWebApp.Controllers.Test;

public class ParserPathControllerTest : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory;
	private readonly HttpClient _client;

	public ParserPathControllerTest(WebApplicationFactory<Program> factory) {
		_factory = factory;
		_client = _factory.CreateClient();
	}

	[Fact]
	public async Task Default_ParsePathByEndpointNameでデフォルトのルートでパースする() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/parserpath/default");
		var json = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		Assert.NotNull(json);
		Assert.Collection(
			json?.OrderBy(item => item.Key),
			item => {
				Assert.Equal("action", item.Key);
				Assert.Equal("index", item.Value, StringComparer.OrdinalIgnoreCase);
			},
			item => {
				Assert.Equal("controller", item.Key);
				Assert.Equal("sample", item.Value, StringComparer.OrdinalIgnoreCase);
			},
			item => {
				Assert.Equal("id", item.Key);
				Assert.Equal("1", item.Value, StringComparer.Ordinal);
			});
	}

	[Fact]
	public async Task Anothert_ParsePathByEndpointNameでRoute指定したルートでパースする() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/parserpath/anotherroute");
		var json = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		Assert.NotNull(json);
		Assert.Collection(
			json?.OrderBy(item => item.Key),
			item => {
				Assert.Equal("action", item.Key);
				Assert.Equal("another", item.Value, StringComparer.OrdinalIgnoreCase);
			},
			item => {
				Assert.Equal("controller", item.Key);
				Assert.Equal("parserpath", item.Value, StringComparer.OrdinalIgnoreCase);
			},
			item => {
				Assert.Equal("x", item.Key);
				Assert.Equal("a", item.Value, StringComparer.Ordinal);
			},
			item => {
				Assert.Equal("y", item.Key);
				Assert.Equal("b", item.Value, StringComparer.Ordinal);
			},
			item => {
				Assert.Equal("z", item.Key);
				Assert.Equal("c", item.Value, StringComparer.Ordinal);
			});
	}
}
