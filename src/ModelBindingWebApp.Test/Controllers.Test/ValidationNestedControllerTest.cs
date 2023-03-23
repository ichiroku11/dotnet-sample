using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class ValidationNestedControllerTest : ControllerTestBase {
	public ValidationNestedControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory) : base(output, factory) {
	}

	// POSTするJSON、バリデーションエラーのキーとメッセージ
	public static TheoryData<object, string, string> GetTheoryData_Nested() {
		return new() {
			// 空のJSONをPOSTする
			// OuterModel.Innerがnullになり、OuterModel.Innerに設定されたRequired属性のバリデーションエラーになる
			{
				new {},
				"Inner",
				"Inner is required."
			},
			// innerプロパティは存在するが中身は空のJSONをPOSTする
			// OuterModel.Inner.Valueがnullになり、InnerModel.Valueに設定されたRequired属性のバリデーションエラーになる
			{
				new { inner = new { } },
				"Inner.Value",
				"Value is required."
			},
			// inner.valueプロパティが空文字のJSONをPOSTする
			// OuterModel.Inner.Valueが空文字になり、InnerModel.Valueに設定されたRequired属性のバリデーションエラーになる
			{
				new { inner = new { value = "" } },
				"Inner.Value",
				"Value is required."
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Nested))]
	public async Task Nested_ネストされたモデルのバリデーションエラーを確認する(object jsonToPost, string expectedErrorKey, string expectedErrorMessage) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/nested", jsonToPost);
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.Collection(
			problem!.Errors.OrderBy(error => error.Key),
			entry => {
				Assert.Equal(expectedErrorKey, entry.Key);
				Assert.Single(entry.Value, expectedErrorMessage);
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}
}
