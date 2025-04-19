using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit.Abstractions;

namespace AntiforgeryWebApp.Controllers.Test;

public class AntiforgeryControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: IClassFixture<WebApplicationFactory<Program>> {

	private readonly ITestOutputHelper _output = output;
	private readonly WebApplicationFactory<Program> _factory = factory;

	private class TokenSet {
		public static readonly TokenSet Empty = new();

		public string? RequestToken { get; init; }
		public string FormFieldName { get; init; } = "";
		public string? HeaderName { get; init; }
		public string? CookieToken { get; init; }
	}

	private class Paths {
		public const string GetTokens = "/antiforgery/gettokens";
		public const string GetAndStoreTokens = "/antiforgery/getandstoretokens";
		public const string ValidateRequest = "/antiforgery/validaterequest";
	}

	[Fact]
	public async Task GetTokens_レスポンスを確認する() {
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync(Paths.GetTokens);
		_output.WriteLine(response.ToString());

		var tokenSet = await response.Content.ReadFromJsonAsync<TokenSet>();

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
		var tokenSet1 = await response1.Content.ReadFromJsonAsync<TokenSet>();

		var response2 = await client.GetAsync(Paths.GetAndStoreTokens);
		_output.WriteLine(response2.ToString());
		var tokenSet2 = await response2.Content.ReadFromJsonAsync<TokenSet>();

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

		// 1回目と2回目でリクエストトークンは異なる
		Assert.NotEqual(tokenSet1.RequestToken, tokenSet2.RequestToken);
	}

	[Fact]
	public async Task GetAndStoreTokens_Cookieヘッダーを付与せず2回呼び出した場合をレスポンスを確認する() {
		// Arrange
		var client = _factory.CreateClient(new WebApplicationFactoryClientOptions {
			HandleCookies = false,
		});

		// Act
		var response1 = await client.GetAsync(Paths.GetAndStoreTokens);
		_output.WriteLine(response1.ToString());
		var tokenSet1 = await response1.Content.ReadFromJsonAsync<TokenSet>();

		var response2 = await client.GetAsync(Paths.GetAndStoreTokens);
		_output.WriteLine(response2.ToString());
		var tokenSet2 = await response2.Content.ReadFromJsonAsync<TokenSet>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
		Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

		// クッキートークンが含まれる
		Assert.NotNull(tokenSet1);
		Assert.NotNull(tokenSet1.CookieToken);
		Assert.NotEmpty(tokenSet1.CookieToken);

		// 2回目の呼び出しでもクッキートークンは含まれる
		Assert.NotNull(tokenSet2);
		Assert.NotNull(tokenSet2.CookieToken);
		Assert.NotEmpty(tokenSet2.CookieToken);

		// 1回目と2回目でリクエストトークンは異なる
		Assert.NotEqual(tokenSet1.RequestToken, tokenSet2.RequestToken);

		// 1回目と2回目でクッキートークンも異なる
		Assert.NotEqual(tokenSet1.CookieToken, tokenSet2.CookieToken);
	}

	private async Task<TokenSet> GetAndStoreTokensAsync(HttpClient client) {
		var response = await client.GetAsync(Paths.GetAndStoreTokens);
		var tokenSet = await response.Content.ReadFromJsonAsync<TokenSet>();

		_output.WriteLine(response.ToString());
		_output.WriteLine(JsonSerializer.Serialize(tokenSet));

		return tokenSet ?? TokenSet.Empty;
	}

	[Fact]
	public async Task ValidateRequestAsync_リクエストヘッダーにクッキートークンが含まれないのでレスポンスはBadRequest() {
		// Arrange
		var client = _factory.CreateClient(new WebApplicationFactoryClientOptions {
			HandleCookies = false,
		});

		var _ = await GetAndStoreTokensAsync(client);

		// Act
		var response = await client.PostAsync(Paths.ValidateRequest, new FormUrlEncodedContent([]));
		var content = await response.Content.ReadAsStringAsync();
		_output.WriteLine(response.ToString());
		_output.WriteLine(content);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task ValidateRequestAsync_リクエストヘッダーにクッキートークンが含まれているがポストデータにリクエストトークンが含まれないのでレスポンスはBadRequest() {
		// Arrange
		var client = _factory.CreateClient();

		var _ = await GetAndStoreTokensAsync(client);

		// Act
		// POSTデータにリクエストトークンを含めない
		var response = await client.PostAsync(Paths.ValidateRequest, new FormUrlEncodedContent([]));
		var content = await response.Content.ReadAsStringAsync();
		_output.WriteLine(response.ToString());
		_output.WriteLine(content);

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task ValidateRequestAsync_リクエストヘッダーにクッキートークンが含まれポストデータにリクエストトークンが含めるとレスポンスはOK() {
		// Arrange
		var client = _factory.CreateClient();

		var tokenSet = await GetAndStoreTokensAsync(client);

		// Act
		// POSTデータにリクエストトークンを含める
		var response = await client.PostAsync(
			Paths.ValidateRequest,
			new FormUrlEncodedContent(new Dictionary<string, string> {
				[tokenSet.FormFieldName] = tokenSet.RequestToken ?? "",
			}));
		var content = await response.Content.ReadAsStringAsync();
		_output.WriteLine(response.ToString());
		_output.WriteLine(content);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);

		// POSTした結果のレスポンスにSetCookieヘッダーは存在しない
		Assert.False(response.Headers.Contains(HeaderNames.SetCookie));
	}
}
