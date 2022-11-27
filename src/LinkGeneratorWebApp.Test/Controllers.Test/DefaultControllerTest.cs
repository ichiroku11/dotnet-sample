using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace LinkGeneratorWebApp.Controllers.Test;

public class DefaultControllerTest : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory;

	public DefaultControllerTest(WebApplicationFactory<Program> factory) {
		_factory = factory;
	}

	[Fact]
	public async Task Index() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
