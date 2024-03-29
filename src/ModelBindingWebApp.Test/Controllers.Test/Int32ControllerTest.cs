using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class Int32ControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {
	[Fact]
	public async Task GetWithQuery_クエリ文字列を省略するとintは0になる() {
		// Arrange
		var client = CreateClient();
		using var request = new HttpRequestMessage(HttpMethod.Get, "/int32/getwithquery");

		// Act
		using var response = await client.SendAsync(request);
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal("0", content);
	}

	[Fact]
	public async Task GetWithRoute_ルートのパラメータを省略するとNotFound() {
		// Arrange
		var client = CreateClient();

		using var request = new HttpRequestMessage(HttpMethod.Get, "/int32/getwithroute");

		// Act
		using var response = await client.SendAsync(request);

		// Assert
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}

	[Fact]
	public async Task Post_フォームデータを省略するとintは0になる() {
		// Arrange
		var client = CreateClient();

		using var request = new HttpRequestMessage(HttpMethod.Post, "/int32/post");

		// Act
		using var response = await client.SendAsync(request);
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal("0", content);
	}
}
