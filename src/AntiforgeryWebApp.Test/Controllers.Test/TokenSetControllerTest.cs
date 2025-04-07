using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace AntiforgeryWebApp.Controllers.Test;

public class TokenSetControllerTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory = factory;

	[Fact]
	public async Task GetTokens() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/tokenset/gettokens");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
