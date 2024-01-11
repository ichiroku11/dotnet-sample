using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class ValidationNestedControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {

	// POSTするJSON、バリデーションエラーのキーとメッセージ
	public static TheoryData<object, string, string> GetTheoryData_Nested() {
		return new() {
			// 空のJSONをPOSTする
			// OuterModel.Innerがnullになり、OuterModel.Innerに設定されたRequired属性のバリデーションエラーになる
			{
				new {},
				"Inner",
				"OuterModel.Inner is required."
			},
			// innerプロパティは存在するが中身は空のJSONをPOSTする
			// OuterModel.Inner.Valueがnullになり、InnerModel.Valueに設定されたRequired属性のバリデーションエラーになる
			{
				new { inner = new { } },
				"Inner.Value",
				"InnerModel.Value is required."
			},
			// inner.valueプロパティが空文字のJSONをPOSTする
			// OuterModel.Inner.Valueが空文字になり、InnerModel.Valueに設定されたRequired属性のバリデーションエラーになる
			{
				new { inner = new { value = "" } },
				"Inner.Value",
				"InnerModel.Value is required."
			},
		};
	}

	// ネストされたモデルに対する属性バリデーションが実行されることを確認する
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

	// POSTするJSON、バリデーションエラーのキーとメッセージ
	public static TheoryData<object, string, string> GetTheoryData_ValidatableNested() {
		return new() {
			// 空のJSONをPOSTする
			{
				new {},
				"Inner",
				"ValidatableOuterModel.Inner is required."
			},
			// innerプロパティは存在するが中身は空のJSONをPOSTする
			{
				new { inner = new { } },
				"Inner.Value",
				"ValidatableInnerModel.Value is required."
			},
			// inner.valueプロパティが空文字のJSONをPOSTする
			{
				new { inner = new { value = "" } },
				"Inner.Value",
				"ValidatableInnerModel.Value is required."
			},
		};
	}

	// ネストされたモデルに対するIValidatableObjectによるバリデーションが実行されることを確認する
	[Theory]
	[MemberData(nameof(GetTheoryData_ValidatableNested))]
	public async Task ValidatableNested_ネストされたモデルのバリデーションエラーを確認する(object jsonToPost, string expectedErrorKey, string expectedErrorMessage) {
		// Arrange
		var client = CreateClient();

		// Act
		var response = await client.PostAsJsonAsync("/api/validation/validatablenested", jsonToPost);
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
