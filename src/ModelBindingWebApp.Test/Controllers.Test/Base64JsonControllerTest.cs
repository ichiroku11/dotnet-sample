
using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Models;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class Base64JsonControllerTest : ControllerTestBase {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new() {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	public Base64JsonControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
		: base(output, factory) {
	}

	[Fact]
	public async Task PostAsync_Base64にエンコードしたJSON文字列をモデルにバインドできる() {
		// Arrange
		var expected = new Sample { Id = 1, Name = "a" };
		var json = JsonSerializer.Serialize(expected, _jsonSerializerOptions);
		var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

		using var request = new HttpRequestMessage(HttpMethod.Post, "api/base64json") {
			Content = new FormUrlEncodedContent(new Dictionary<string, string>() { ["sample"] = base64 }),
		};

		// Act
		var response = await SendAsync(request);
		var actual = JsonSerializer.Deserialize<Sample>(await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);

		// Assert
		Assert.Equal(expected, actual);
	}
}
