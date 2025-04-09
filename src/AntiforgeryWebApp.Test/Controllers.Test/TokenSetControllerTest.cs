using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace AntiforgeryWebApp.Controllers.Test;

public class TokenSetControllerTest(
	ITestOutputHelper output,
	WebApplicationFactory<Program> factory)
	: IClassFixture<WebApplicationFactory<Program>> {

	private readonly ITestOutputHelper _output = output;
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
		_output.WriteLine(response.ToString());

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

	[Fact]
	public async Task GetTokens_レスポンスのクッキーヘッダーが存在しないことを確認する() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/tokenset/gettokens");
		_output.WriteLine(response.ToString());

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Empty(response.Headers);
	}
}
