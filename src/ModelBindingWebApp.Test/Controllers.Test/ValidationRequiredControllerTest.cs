using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ModelBindingWebApp.Controllers.Test;

public class ValidationRequiredControllerTest : ControllerTestBase {
	public ValidationRequiredControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory) : base(output, factory) {
	}

	[Fact]
	public async Task ValueType_Required属性が指定された値型のプロパティは値が送信されてなくもバリデーションエラーにならない() {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/required/valuetype", new { });

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
