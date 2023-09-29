using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Composition.Hosting.Core;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class MessageProviderControllerTest : ControllerTestBase {
	public MessageProviderControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory) : base(output, factory) {
	}

	[Fact]
	public async Task MissingBindRequired_テスト() {
		// Arrange
		var client = CreateClient(
			configure: services => {
				services.Configure<MvcOptions>(options => {
					// todo:
				});
			});

		// Act
		var response = await client.PostAsync(
			"/api/messageprovider/missingbindrequired",
			new FormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>()));
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.NotNull(problem);
		var error = Assert.Single(problem.Errors);
		Assert.Equal("Value", error.Key);
		Assert.Equal("A value for the 'Value' parameter or property was not provided.", error.Value.Single());
	}
}
