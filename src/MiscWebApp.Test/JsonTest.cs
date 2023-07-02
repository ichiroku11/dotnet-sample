using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApp.Test;

public class JsonTest : TestBase {
	public JsonTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
		: base(output, factory) {
	}

	[Fact]
	public async Task ReadFromJsonAsyncとWriteAsJsonAsyncを確認する() {
		// Arrange
		var expected = new Sample { Value = 1 };
		var request = new HttpRequestMessage(HttpMethod.Post, "/json") {
			Content = JsonContent.Create(expected, options: JsonHelper.Options)
		};

		// Act
		var response = await SendAsync(request);
		var actual = await response.Content.ReadFromJsonAsync<Sample>(JsonHelper.Options);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(1, actual?.Value);
	}
}
