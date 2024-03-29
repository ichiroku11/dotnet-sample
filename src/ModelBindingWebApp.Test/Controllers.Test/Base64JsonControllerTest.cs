
using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Models;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class Base64JsonControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new() {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	[Fact]
	public async Task PostAsync_Base64にエンコードしたJSON文字列をモデルにバインドできる() {
		// Arrange
		var client = CreateClient();

		var expected = new Sample { Id = 1, Name = "a" };
		var json = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
		var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

		using var request = new HttpRequestMessage(HttpMethod.Post, "api/base64json") {
			Content = new FormUrlEncodedContent(new Dictionary<string, string>() { ["sample"] = base64 }),
		};

		// Act
		var response = await client.SendAsync(request);
		var actual = JsonSerializer.Deserialize<Sample>(await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);

		// Assert
		Assert.Equal(expected, actual);
	}
}
