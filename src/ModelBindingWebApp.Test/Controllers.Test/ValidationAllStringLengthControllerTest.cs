using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class ValidationAllStringLengthControllerTest : ControllerTestBase {
	public ValidationAllStringLengthControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory) : base(output, factory) {
	}

	[Theory]
	[InlineData("01234", "0123456789")]
	public async Task Test_Ok_各文字列が属性で指定した範囲内の文字数の文字列だけが含まれる(params string[] values) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/allstringlength", values);
		var actual = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(actual);
		Assert.Equal(values, actual);
	}

	[Theory]
	[InlineData("01234", "0123456789a")]
	public async Task Test_BadRequest_属性で指定した範囲外の文字数の文字列が含まれる(params string[] values) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/allstringlength", values);
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
}
