using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ModelBindingWebApp.Controllers.Test;

// Required属性
public class ValidationRequiredControllerTest : ControllerTestBase {
	public ValidationRequiredControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory) : base(output, factory) {
	}

	// 値型
	[Fact]
	public async Task ValueType_Required属性が指定された値型のプロパティは値が送信されてなくもバリデーションエラーにならない() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/required/valuetype", new { });

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	// null許容値型
	[Fact]
	public async Task NullableValueType_Required属性が指定されたnull許容値型のプロパティは値が送信されないとバリデーションエラーになる() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/required/nullablevaluetype", new { });
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.Collection(
			problem!.Errors.OrderBy(error => error.Key),
			entry => {
				Assert.Equal("Value", entry.Key);
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}

	// 参照型
	[Fact]
	public async Task ReferenceType_Required属性が指定された参照型のプロパティは値が送信されないとバリデーションエラーになる() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/required/referencetype", new { });
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.Collection(
			problem!.Errors.OrderBy(error => error.Key),
			entry => {
				Assert.Equal("Value", entry.Key);
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}

	// null許容参照型
	[Fact]
	public async Task NullableReferenceType_Required属性が指定されたnull許容参照型のプロパティは値が送信されないとバリデーションエラーになる() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/required/nullablereferencetype", new { });
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.Collection(
			problem!.Errors.OrderBy(error => error.Key),
			entry => {
				Assert.Equal("Value", entry.Key);
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}

	// 試しところ、Required属性に関係ないバリデーションエラーになる様子
	// intに変換できない文字列を送信する
	[Fact]
	public async Task ValueType_値型のプロパティに変換できない値を送信するとバリデーションエラーになる() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/required/valuetype", new { value = "x" });
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.Collection(
			problem!.Errors.OrderBy(error => error.Key),
			entry => {
				Assert.Equal("$.value", entry.Key);
				// The JSON value could not be converted to ModelBindingWebApp.Controllers.ValidationRequiredController + ValueTypeModel.Path: $.value | LineNumber: 0 | BytePositionInLine: 12.
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			},
			entry => {
				Assert.Equal("model", entry.Key);
				// The model field is required.
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}
}
