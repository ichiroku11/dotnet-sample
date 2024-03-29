using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Controllers.Test;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ModelBindingWebApp.Controllers;

public class ValidationCollectionCountControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {
	[Fact]
	public async Task Test_BadRequest_空のJSON() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/collectioncount", new { });
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.NotNull(problem);
		Assert.Collection(
			problem.Errors.OrderBy(error => error.Key),
			entry => {
				Assert.Equal("$", entry.Key);
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			},
			entry => {
				Assert.Equal("values", entry.Key);
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}

	[Theory]
	[InlineData(new[] { 1 })]
	[InlineData(new[] { 1, 2, 3, 4 })]
	public async Task Test_BadRequest_配列の要素数が属性で指定した最大最小の範囲外(int[] values) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/collectioncount", values);
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.NotNull(problem);
		Assert.Collection(
			problem.Errors.OrderBy(error => error.Key),
			entry => {
				Assert.Equal("", entry.Key);
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}

	[Theory]
	[InlineData(new[] { 1, 2 })]
	[InlineData(new[] { 1, 2, 3 })]
	public async Task Test_Ok_配列の要素数が属性で指定した最大最小値内(int[] values) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/collectioncount", values);
		var actual = await response.Content.ReadFromJsonAsync<IEnumerable<int>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(actual);
		Assert.Equal(values, actual);
	}
}
