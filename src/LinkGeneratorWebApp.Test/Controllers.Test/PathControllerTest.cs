using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace LinkGeneratorWebApp.Controllers.Test;

public class PathControllerTest : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory;

	public PathControllerTest(WebApplicationFactory<Program> factory) {
		_factory = factory;
	}

	[Fact]
	public async Task Self_GetPathByActionの引数にHttpContextを指定して絶対パスを生成する() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/path/self");
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("/path/self", content);
	}
}
