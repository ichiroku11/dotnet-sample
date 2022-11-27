using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace LinkGeneratorWebApp.Controllers.Test;

public class UriControllerTest : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory;
	private readonly HttpClient _client;

	public UriControllerTest(WebApplicationFactory<Program> factory) {
		_factory = factory;
		_client = _factory.CreateClient();
	}

	[Fact]
	public async Task Self_GetUriByActionの引数にHttpContextを指定して絶対URLを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/uri/self");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("http://localhost/uri/self", content);
	}

	[Fact]
	public async Task AnotherControllerAction_GetUriByActionの引数にHttpContextを指定せず絶対パスを生成する() {
		// Arrange

		// Act
		var response = await _client.GetAsync("/uri/anothercontrolleraction");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("https://localhost/another", content);
	}

}
