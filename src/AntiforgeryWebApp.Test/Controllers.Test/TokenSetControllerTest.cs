using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace AntiforgeryWebApp.Controllers.Test;

public class TokenSetControllerTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {

	private readonly WebApplicationFactory<Program> _factory = factory;

	private class AntiforgeryTokenSet {
		public string? RequestToken { get; init; }
		public string FormFieldName { get; init; } = "";
		public string? HeaderName { get; init; }
		public string? CookieToken { get; init; }
	}

	[Fact]
	public async Task GetTokens() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/tokenset/gettokens");
		var tokens = await response.Content.ReadFromJsonAsync<AntiforgeryTokenSet>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(tokens);
		Assert.NotNull(tokens.RequestToken);
		Assert.NotNull(tokens.FormFieldName);
		Assert.NotNull(tokens.HeaderName);
		Assert.NotNull(tokens.CookieToken);
	}
}
