using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApi.Controllers.Test;

public class RequestBodyControllerTest : ControllerTestBase {
	public RequestBodyControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
		: base(output, factory) {
	}

	private class Result {
		public string Body { get; init; } = "";

		public string Value { get; init; } = "";
	}

	[Fact]
	public async Task JsonWithBind_モデルにバインド済みなのでRequestBodyの中身は空文字() {
		// Arrange
		var client = CreateClient();

		// Act
		using var response = await client.PostAsJsonAsync("/api/requestbody/jsonwithbind", new { value = "abc" });
		var actual = await response.Content.ReadFromJsonAsync<Result>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(actual);
		Assert.Empty(actual.Body);
		Assert.Equal("abc", actual.Value);
	}

	[Fact]
	public async Task JsonWithoutBind_モデルにバインドしていないのでRequestBodyから読み取れる() {
		// Arrange
		var client = CreateClient();

		// Act
		using var response = await client.PostAsJsonAsync("/api/requestbody/jsonwithoutbind", new { value = "abc" });
		var actual = await response.Content.ReadFromJsonAsync<Result>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(actual);
		Assert.Equal(@"{""value"":""abc""}", actual.Body);
		Assert.Empty(actual.Value);
	}

	[Fact]
	public async Task FormWithBind_モデルにバインド済みなのでRequestBodyの中身は空文字() {
		// Arrange
		var client = CreateClient();

		// Act
		using var response = await client.PostAsync(
			"/api/requestbody/formwithbind",
			new FormUrlEncodedContent(new[] { KeyValuePair.Create("value", "abc") }));
		var actual = await response.Content.ReadFromJsonAsync<Result>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(actual);
		Assert.Empty(actual.Body);
		Assert.Equal("abc", actual.Value);
	}
}
