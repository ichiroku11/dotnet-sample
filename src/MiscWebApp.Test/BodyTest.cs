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
		public bool Thrown { get; set; }
	}

	[Fact]
	public async Task EnableBufferingを使わないとリクエストボディは2回読み取れないことを確認する() {
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
		Assert.True(actual.Thrown);
	}

	[Fact]
	public async Task EnableBufferingを使うとリクエストボディは2回読み取れることを確認する() {
		// Arrange
		var request = new HttpRequestMessage(HttpMethod.Post, "/body?buffering=true") {
			Content = new StringContent("content"),
		};

		// Act
		var response = await SendAsync(request);
		var actual = await response.Content.ReadFromJsonAsync<Result>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(actual);
		Assert.True(actual.CanSeek);
		Assert.Equal("content", actual.First);
		Assert.Equal("content", actual.Second);
		Assert.False(actual.Thrown);
	}
}
