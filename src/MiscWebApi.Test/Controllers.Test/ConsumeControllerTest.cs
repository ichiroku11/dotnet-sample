using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http.Headers;

namespace MiscWebApi.Controllers.Test;

// 参考
// https://learn.microsoft.com/ja-jp/aspnet/core/web-api/?view=aspnetcore-7.0#define-supported-request-content-types-with-the-consumes-attribute-1
public class ConsumeControllerTest : ControllerTestBase {
	public ConsumeControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory) : base(output, factory) {
	}

	[Theory]
	// Consumes属性がない場合
	// "application/json"、"text/json"を受け入れる
	[InlineData("application/json", HttpStatusCode.OK)]
	[InlineData("text/json", HttpStatusCode.OK)]
	// "text/plain"は受け入れない
	// 受け入れない場合は415のステータスコードが返る
	[InlineData("text/plain", HttpStatusCode.UnsupportedMediaType)]
	public async Task Default_Consumes属性を指定しない場合の動きを確認する(string mediaType, HttpStatusCode expected) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsync(
			"/api/consume/default",
			JsonContent.Create(
				new { value = "x" },
				new MediaTypeHeaderValue(mediaType) { CharSet = "utf-8" }));

		// Assert
		Assert.Equal(expected, response.StatusCode);
	}


	[Theory]
	// Consumes属性で指定した"application/json"のみ受け入れる
	[InlineData("application/json", HttpStatusCode.OK)]
	[InlineData("text/json", HttpStatusCode.UnsupportedMediaType)]
	[InlineData("text/plain", HttpStatusCode.UnsupportedMediaType)]
	public async Task ApplicationJson_Consumes属性を指定した場合の動きを確認する(string mediaType, HttpStatusCode expected) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsync(
			"/api/consume/applicationjson",
			JsonContent.Create(
				new { value = "x" },
				new MediaTypeHeaderValue(mediaType) { CharSet = "utf-8" }));

		// Assert
		Assert.Equal(expected, response.StatusCode);
	}

}
