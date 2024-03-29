using Microsoft.AspNetCore.Mvc.Testing;
using MiscWebApi.Models;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApi.Controllers.Test;

public class UserControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {

	// 認証されていないのでUnauthorized
	[Fact]
	public async Task UserMeResponse_Unauthorized() {
		// Arrange
		var client = CreateClient();
		using var request = new HttpRequestMessage(HttpMethod.Get, "/api/user/me");

		// Act
		using var response = await client.SendAsync(request);

		// Assert
		Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
	}

	// 認証をエミュレートしているのでOKレスポンス
	[Fact]
	public async Task UserMeResponse_Ok() {
		// Arrange
		var client = CreateClientWithTestAuth();
		using var request = new HttpRequestMessage(HttpMethod.Get, "/api/user/me");

		// Act
		using var response = await client.SendAsync(request);
		var me = await DeserializeAsync<UserMeResponse>(response);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("11", me?.Id);
		Assert.Equal("xx", me?.Name);
	}
}

