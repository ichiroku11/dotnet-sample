using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Xunit;
using System.Net;

namespace MiscWebApi.Controllers.Test;

public class ProduceControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {

	// Produces属性がない場合、レスポンスのContentTypeは"application/json"になる
	[Fact]
	public async Task Default_Produces属性を指定しない場合の動きを確認する() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.GetAsync("/api/produce/default");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
	}

	// Produces属性で"text/json"を指定した場合、レスポンスのContentTypeは"text/json"になる
	[Fact]
	public async Task TextJson_Produces属性を指定した場合の動きを確認する() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.GetAsync("/api/produce/textjson");

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("text/json", response.Content.Headers.ContentType?.MediaType);
	}
}
