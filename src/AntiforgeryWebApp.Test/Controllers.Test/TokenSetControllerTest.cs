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
	public async Task GetTokens_レスポンスを確認する() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/tokenset/gettokens");
		var tokenSet = await response.Content.ReadFromJsonAsync<AntiforgeryTokenSet>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		Assert.NotNull(tokenSet);

		// リクエストトークンとクッキートークンが生成される
		Assert.NotNull(tokenSet.RequestToken);
		Assert.NotNull(tokenSet.CookieToken);

		Assert.NotNull(tokenSet.FormFieldName);
		Assert.NotNull(tokenSet.HeaderName);
	}
}
