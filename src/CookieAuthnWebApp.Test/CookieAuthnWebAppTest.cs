using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace CookieAuthnWebApp;

public class CookieAuthnWebAppTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
	: IClassFixture<WebApplicationFactory<Startup>>, IDisposable {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new() {
			PropertyNameCaseInsensitive = true,
		};

	private readonly ITestOutputHelper _output = output;
	private readonly WebApplicationFactory<Startup> _factory = factory;

	public void Dispose() {
		// だめっぽい
		//_factory.Dispose();
	}

	[Fact]
	public async Task GetChallenge_AccountLoginへのリダイレクト() {
		// Arrange
		var options = new WebApplicationFactoryClientOptions {
			AllowAutoRedirect = false,
		};
		using var client = _factory.CreateClient(options);

		// Act
		using var response = await client.GetAsync("/challenge");

		// Assert
		Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
		Assert.Equal(
			new Uri(_factory.Server.BaseAddress, "/account/login").ToString(),
			response.Headers.Location?.GetLeftPart(UriPartial.Path));
	}

	[Fact]
	public async Task GetForbid_AccountAccessDeniedへのリダイレクト() {
		// Arrange
		var options = new WebApplicationFactoryClientOptions {
			AllowAutoRedirect = false,
		};
		using var client = _factory.CreateClient(options);

		// Act
		using var response = await client.GetAsync("/forbid");

		// Assert
		Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
		Assert.Equal(
			new Uri(_factory.Server.BaseAddress, "/account/accessdenied").ToString(),
			response.Headers.Location?.GetLeftPart(UriPartial.Path));
	}

	private class AuthenticateResult {
		public bool Succeeded { get; init; }
		public string NameIdentifier { get; init; } = "";
		public string Role { get; init; } = "";
		public string Item { get; init; } = "";
		public string Parameter { get; init; } = "";
	}

	[Fact]
	public async Task GetAuthenticate_サインインしていない状態で正しい結果を取得できる() {
		// Arrange
		using var client = _factory.CreateClient();

		// Act
		using var response = await client.GetAsync("/authenticate");
		var content = await response.Content.ReadAsStringAsync();
		_output.WriteLine(content);

		var result = JsonSerializer.Deserialize<AuthenticateResult>(content, _jsonSerializerOptions)!;

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.False(result.Succeeded);
		Assert.Equal("", result.NameIdentifier);
		Assert.Equal("", result.Role);
		Assert.Equal("", result.Item);
		Assert.Equal("", result.Parameter);
	}

	[Fact]
	public async Task GetAuthenticate_サインインした状態で正しい結果を取得できる() {
		// Arrange
		var options = new WebApplicationFactoryClientOptions {
		};
		var client = _factory.CreateClient(options);

		// Act
		await client.GetAsync("/signin");

		var response = await client.GetAsync("/authenticate");
		var content = await response.Content.ReadAsStringAsync();
		_output.WriteLine(content);

		var result = JsonSerializer.Deserialize<AuthenticateResult>(content, _jsonSerializerOptions)!;

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.True(result.Succeeded);
		Assert.Equal("1", result.NameIdentifier);
		Assert.Equal("Admin", result.Role);
		Assert.Equal("2", result.Item);
		Assert.Equal("", result.Parameter);
	}

	[Fact]
	public async Task GetSignin_レスポンスにSetCookieヘッダが含まれる() {
		// Arrange
		using var client = _factory.CreateClient();

		// Act
		using var response = await client.GetAsync("/signin");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.True(response.Headers.Contains(HeaderNames.SetCookie));
		foreach (var value in response.Headers.GetValues(HeaderNames.SetCookie)) {
			_output.WriteLine(value);
		}
	}

	[Fact]
	public async Task GetSignout_レスポンスにSetCookieヘッダが含まれる() {
		// Arrange
		using var client = _factory.CreateClient();

		// Act
		using var response = await client.GetAsync("/signout");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.True(response.Headers.Contains(HeaderNames.SetCookie));
		foreach (var value in response.Headers.GetValues(HeaderNames.SetCookie)) {
			_output.WriteLine(value);
		}
	}
}
