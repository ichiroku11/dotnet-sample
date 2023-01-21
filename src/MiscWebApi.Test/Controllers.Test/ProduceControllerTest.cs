using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Xunit;
using System.Net;

namespace MiscWebApi.Controllers.Test;

public class ProduceControllerTest : ControllerTestBase {
	public ProduceControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory) : base(output, factory) {
	}

	// Produces属性がない場合、"application/json"になる
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
}
