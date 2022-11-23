using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace LinkGeneratorWebApp.Controllers.Test;

public class PathControllerTest : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory;
	private readonly HttpClient _client;

	public PathControllerTest(WebApplicationFactory<Program> factory) {
		_factory = factory;
		_client = _factory.CreateClient();
	}

	[Fact]
	public async Task Self_GetPathByActionの引数にHttpContextを指定して絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/path/self");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/path/self", content);
	}

	[Fact]
	public async Task SelfWithoutHttpContext_GetPathByActionの引数にHttpContextを指定せずに絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/path/selfwithouthttpcontext");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/path/selfwithouthttpcontext", content);
	}

	[Fact]
	public async Task AnotherAction_GetPathByActionの引数にHttpContextとactionを指定して絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/path/anotheraction");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/path/another", content);
	}

	[Fact]
	public async Task AnotherController_GetPathByActionの引数にHttpContextとcontrollerを指定して絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/path/anothercontroller");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/another/anothercontroller", content);
	}

	[Fact]
	public async Task AnotherControllerAction_GetPathByActionの引数にHttpContextとactionとcontrollerを指定して絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/path/anothercontrolleraction");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/another", content);
	}
}
