using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using System.Net.Http.Headers;

namespace MiscWebApi.Controllers.Test;

public class ConsumeControllerTest : ControllerTestBase {
	public ConsumeControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory) : base(output, factory) {
	}

	[Fact]
	public async Task Default_() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsync(
			"/api/consume/default",
			JsonContent.Create(
				new { value = "x" },
				new MediaTypeHeaderValue("application/json") { CharSet = "utf-8" }));

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
