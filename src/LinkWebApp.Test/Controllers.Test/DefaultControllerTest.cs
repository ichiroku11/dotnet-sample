using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace LinkWebApp.Controllers.Test;

public class DefaultControllerTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory = factory;

	[Fact]
	public async Task Generator() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/default/generator");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/sample", content);
	}

	[Fact]
	public async Task Parser() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/default/parser");
		var json = await response.Content.ReadFromJsonAsync<IDictionary<string, string>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		Assert.NotNull(json);
		Assert.Equal(2, json!.Count);

		var controller = Assert.Contains("controller", json);
		Assert.Equal("sample", controller, StringComparer.OrdinalIgnoreCase);

		var action = Assert.Contains("action", json);
		Assert.Equal("index", action, StringComparer.OrdinalIgnoreCase);
	}
}
