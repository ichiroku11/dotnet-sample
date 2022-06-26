using Microsoft.AspNetCore.Mvc.Testing;
using MiscWebApi.Models;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApi.Controllers.Test;

public class UserControllerTest : ControllerTestBase {
	public UserControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
		: base(output, factory) {
	}

	[Fact]
	public async Task UserMeResponse_Ok() {
		// Arrange
		var client = CreateClient();
		using var request = new HttpRequestMessage(HttpMethod.Get, "/api/user/me");

		// Act
		using var response = await client.SendAsync(request);
		var me = await DeserializeAsync<UserMeResponse>(response);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("x", me?.Id);
		Assert.Equal("xx", me?.Name);
	}
}

