using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

// BindRequired属性
public class ValidationBindRequiredControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {

	// 値型
	[Fact]
	public async Task ValueType_BindRequired属性が指定された値型のプロパティは値が送信されてなくもバリデーションエラーにならない() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/bindrequired/valuetype", new { });

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	// null許容値型
	[Fact]
	public async Task NullableValueType_BindRequired属性が指定されたnull許容値型のプロパティは値が送信されてなくもバリデーションエラーにならない() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/bindrequired/nullablevaluetype", new { });

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	// 参照型
	[Fact]
	public async Task ReferenceType_BindRequired属性が指定された参照型のプロパティは値が送信されてなくもバリデーションエラーにならない() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/bindrequired/referencetype", new { });

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	// null許容参照型
	[Fact]
	public async Task NullableReferenceType_BindRequired属性が指定されたnull許容参照型のプロパティは値が送信されてなくもバリデーションエラーにならない() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/bindrequired/nullablereferencetype", new { });

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
