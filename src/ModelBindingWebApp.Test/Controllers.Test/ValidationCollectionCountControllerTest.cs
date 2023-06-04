using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Controllers.Test;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ModelBindingWebApp.Controllers;

public class ValidationCollectionCountControllerTest : ControllerTestBase {
	public ValidationCollectionCountControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory) : base(output, factory) {
	}

	[Theory]
	[InlineData(new[] { 1, 2 })]
	[InlineData(new[] { 1, 2, 3 })]
	public async Task Test_Ok_属性で指定した最大最小値内(int[] values) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/collectioncount", new { values });
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
