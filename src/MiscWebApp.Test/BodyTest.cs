using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApp.Test;

public class BodyTest : TestBase {
	public BodyTest(ITestOutputHelper output, WebApplicationFactory<Program> factory) : base(output, factory) {
	}

	private class Result {
		public bool CanSeek { get; init; }
		public string First { get; init; } = "";
		public string Second { get; init; } = "";
	}

	[Fact]
	public async Task リクエストボディは2回読み取れないことを確認する() {
		// Arrange
		var request = new HttpRequestMessage(HttpMethod.Post, "/body") {
			Content = new StringContent("content"),
		};

		// Act
		var response = await SendAsync(request);
		var actual = await response.Content.ReadFromJsonAsync<Result>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(actual);
		Assert.False(actual.CanSeek);
		Assert.Equal("content", actual.First);
		Assert.Equal("", actual.Second);
	}
}
