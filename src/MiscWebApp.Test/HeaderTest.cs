using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApp.Test;

public class HeaderTest : TestBase {
	public HeaderTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
		: base(output, factory) {
	}

	[Fact]
	public async Task テスト実行だとレスポンスヘッダがなさげ() {
		// Arrange
		var request = new HttpRequestMessage(HttpMethod.Get, "/header");

		// Act
		var response = await SendAsync(request);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Empty(response.Headers);
	}
}
