using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace LinkWebApp.Controllers.Test;

public class GeneratorUriControllerTest : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory;
	private readonly HttpClient _client;

	public GeneratorUriControllerTest(WebApplicationFactory<Program> factory) {
		_factory = factory;
		_client = _factory.CreateClient();
	}

	[Fact]
	public async Task Self_GetUriByActionの引数にHttpContextを指定して絶対URLを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/generatoruri/self");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("http://localhost/generatoruri/self", content);
	}

	[Fact]
	public async Task AnotherControllerAction_GetUriByActionの引数にHttpContextを指定せず絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/generatoruri/anothercontrolleraction");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("https://localhost/another", content);
	}

}
