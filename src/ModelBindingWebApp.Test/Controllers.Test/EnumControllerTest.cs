using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class EnumControllerTest : ControllerTestBase {
	public EnumControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
		: base(output, factory) {
	}

	[Theory]
	[InlineData("1")]
	[InlineData("apple")]
	[InlineData("Apple")]
	public async Task Get_Enumにバインドできる(string fruit) {
		// Arrange
		var client = CreateClient();

		using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/enum/{fruit}");

		// Act
		using var response = await client.SendAsync(request);
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.Equal("1", content);
	}
}
