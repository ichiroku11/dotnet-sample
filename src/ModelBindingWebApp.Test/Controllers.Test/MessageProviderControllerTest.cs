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

	private static FormUrlEncodedContent GetEmptyFormUrlEncodedContent()
		=> new(Enumerable.Empty<KeyValuePair<string, string>>());

	public MessageProviderControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory) : base(output, factory) {
	}

	[Fact]
	public async Task MissingBindRequired_BindRequired属性のエラーメッセージを変更できる() {
		// Arrange
		var client = CreateClient(
			configure: services => {
				services.Configure<MvcOptions>(options => {
					// デフォルトのメッセージ
					// "A value for the '{0}' parameter or property was not provided."
					options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(arg => {
						return $@"""{arg}""を指定してください。";
					});
				});
			});

		// Act
		var response = await client.PostAsync(
			"/api/messageprovider/missingbindrequired",
			GetEmptyFormUrlEncodedContent());
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.NotNull(problem);
		var error = Assert.Single(problem.Errors);
		Assert.Equal("Value", error.Key);
		Assert.Equal(@"""Value""を指定してください。", error.Value.Single());
	}
}
