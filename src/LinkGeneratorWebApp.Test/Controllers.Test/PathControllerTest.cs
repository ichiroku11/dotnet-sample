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
	public async Task OtherAction_GetPathByActionの引数にHttpContextとactionを指定して絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/path/otheraction");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/path/other", content);
	}

	[Fact]
	public async Task OtherController_GetPathByActionの引数にHttpContextとcontrollerを指定して絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/path/othercontroller");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/other/othercontroller", content);
	}

	[Fact]
	public async Task OtherControllerAction_GetPathByActionの引数にHttpContextとactionとcontrollerを指定して絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/path/othercontrolleraction");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/other", content);
	}
}
