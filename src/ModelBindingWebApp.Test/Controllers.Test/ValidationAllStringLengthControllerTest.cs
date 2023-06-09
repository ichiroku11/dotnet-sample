using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class ValidationAllStringLengthControllerTest : ControllerTestBase {
	public ValidationAllStringLengthControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory) : base(output, factory) {
	}

	[Theory]
	[InlineData("01234", "0123456789")]
	public async Task Test_Ok_各文字列が属性で指定した最大最小値内の文字数(params string[] values) {
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
}
