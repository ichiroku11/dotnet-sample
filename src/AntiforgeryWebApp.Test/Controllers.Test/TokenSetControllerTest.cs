using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
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

	private class Paths {
		public const string GetTokens = "/tokenset/gettokens";
		public const string GetAndStoreTokens = "/tokenset/getandstoretokens";
	}

	[Fact]
	public async Task GetTokens_レスポンスを確認する() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync(Paths.GetTokens);
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
	public async Task GetTokens_レスポンスにSetCookieヘッダーが存在しないことを確認する() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync(Paths.GetTokens);
		_output.WriteLine(response.ToString());

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Empty(response.Headers);
		Assert.False(response.Headers.Contains(HeaderNames.SetCookie));
	}

	[Fact]
	public async Task GetAndStoreTokens_レスポンスにSetCookieヘッダーが存在することを確認する() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync(Paths.GetAndStoreTokens);
		_output.WriteLine(response.ToString());

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotEmpty(response.Headers);
		Assert.True(response.Headers.Contains(HeaderNames.SetCookie));
	}

	[Fact]
	public async Task GetAndStoreTokens_2回呼び出した場合をレスポンスを確認する() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response1 = await client.GetAsync(Paths.GetAndStoreTokens);
		_output.WriteLine(response1.ToString());
		var tokenSet1 = await response1.Content.ReadFromJsonAsync<AntiforgeryTokenSet>();

		var response2 = await client.GetAsync(Paths.GetAndStoreTokens);
		_output.WriteLine(response2.ToString());
		var tokenSet2 = await response2.Content.ReadFromJsonAsync<AntiforgeryTokenSet>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
		Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

		// クッキートークンが含まれる
		Assert.NotNull(tokenSet1);
		Assert.NotNull(tokenSet1.CookieToken);
		Assert.NotEmpty(tokenSet1.CookieToken);

		// 2回目の呼び出しではクッキートークンは含まれない
		// リクエストにCookieヘッダーが含まれるからだと思われる
		Assert.NotNull(tokenSet2);
		Assert.Null(tokenSet2.CookieToken);

		// リクエストトークンは異なる
		Assert.NotEqual(tokenSet1.RequestToken, tokenSet2.RequestToken);
	}
}
