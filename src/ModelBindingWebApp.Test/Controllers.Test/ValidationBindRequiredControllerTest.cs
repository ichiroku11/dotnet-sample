using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

// BindRequired属性
public class ValidationBindRequiredControllerTest : ControllerTestBase {
	public ValidationBindRequiredControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory) : base(output, factory) {
	}

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
	public async Task NullableValueType_BindRequired属性が指定された値型のプロパティは値が送信されてなくもバリデーションエラーにならない() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/bindrequired/nullablevaluetype", new { });

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}

	// 参照型
	// todo:

	// null許容参照型
	// todo:
}
