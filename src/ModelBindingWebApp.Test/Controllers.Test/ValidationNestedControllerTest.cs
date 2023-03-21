using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class ValidationNestedControllerTest : ControllerTestBase {
	public ValidationNestedControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory) : base(output, factory) {
	}

	[Fact]
	public async Task Nested_空のJSONをPOSTするとInnerModel自体がバインドされない() {
		// Arrange
		var client = CreateClient();

		// Act
		// 空のJSONをPOSTする
		var response = await client.PostAsJsonAsync("/api/validation/nested", new { });
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.Collection(
			problem!.Errors.OrderBy(error => error.Key),
			entry => {
				// OuterModel.Innerがnullになり、
				// OuterModel.Innerに設定されたRequired属性のバリデーションエラーになる
				Assert.Equal("Inner", entry.Key);
				Assert.Single(entry.Value, "Inner is required.");
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}

	[Fact]
	public async Task Nested_innerプロパティが空のJSONをPOSTするとInnerModelはバインドされる() {
		// Arrange
		var client = CreateClient();

		// Act
		// innerプロパティは存在するが中身は空のJSONをPOSTする
		var response = await client.PostAsJsonAsync("/api/validation/nested", new { inner = new { } });
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

		Assert.Collection(
			problem!.Errors.OrderBy(error => error.Key),
			entry => {
				// OuterModel.Inner.Valueがnullになり、
				// InnerModel.Valueに設定されたRequired属性のバリデーションエラーになる
				Assert.Equal("Inner.Value", entry.Key);
				Assert.Single(entry.Value, "Value is required.");
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}
}
