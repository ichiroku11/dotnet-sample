using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
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

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public async Task ValueMustNotBeNull_Required属性のエラーメッセージを変更できる(string? value) {
		// Arrange
		var client = CreateClient(
			configure: services => {
				services.Configure<MvcOptions>(options => {
					// デフォルトのメッセージ
					// "The Value field is required."
					options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(arg => {
						return $@"""{arg}""をバインドできません。";
					});
				});
			});

		// Act
		var response = await client.PostAsync(
			"/api/messageprovider/valuemustnotbenull",
			new FormUrlEncodedContent(new Dictionary<string, string?> { ["value"] = value }));
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.NotNull(problem);
		var error = Assert.Single(problem.Errors);
		Assert.Equal("Value", error.Key);
		Assert.Equal(@"""""をバインドできません。", error.Value.Single());
	}

	[Fact]
	public async Task MissingKeyOrValue_Dictionaryのキーバリューが見つからないときのエラーメッセージを変更できる() {
		// Arrange
		var client = CreateClient(
			configure: services => {
				services.Configure<MvcOptions>(options => {
					// デフォルトのメッセージ
					// A value is required.
					// todo:
				});
			});

		// Act
		var response = await client.PostAsync(
			"/api/messageprovider/missingkeyorvalue",
			new FormUrlEncodedContent(
				new Dictionary<string, string?> {
					[@"values[0].Key"] = "x",
				}));
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.NotNull(problem);
		var error = Assert.Single(problem.Errors);
		Assert.Equal("Values[0].Value", error.Key);
		Assert.Equal("A value is required.", error.Value.Single());
	}
}
