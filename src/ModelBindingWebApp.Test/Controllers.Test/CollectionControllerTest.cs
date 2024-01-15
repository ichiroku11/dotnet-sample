using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Models;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class CollectionControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	public static TheoryData<IEnumerable<KeyValuePair<string, string>>> GetTheoryData_Int32Values() {
		return new() {
			// values=1&values=2
			new[] {
				KeyValuePair.Create("values", "1"),
				KeyValuePair.Create("values", "2"),
			},

			// values[0]=1&values[1]=2
			new Dictionary<string, string>() {
				{ "values[0]", "1" },
				{ "values[1]", "2" },
			},

			// values[0]=1&values[1]=2&values[3]=3
			new Dictionary<string, string>() {
				{ "values[0]", "1" },
				{ "values[1]", "2" },
				// 欠番以降は無視される
				{ "values[3]", "3" },
			},

			// [0]=1&[1]=2
			new Dictionary<string, string>() {
				{ "[0]", "1" },
				{ "[1]", "2" },
			},

			// values[]=1&values[]=2
			new[] {
				KeyValuePair.Create("values[]", "1"),
				KeyValuePair.Create("values[]", "2"),
			},
		};
	}

	[Theory(DisplayName = "IEnumerable<int>型のvaluesにバインドできる")]
	[MemberData(nameof(GetTheoryData_Int32Values))]
	public async Task PostAsync_CanBindToInt32Values(IEnumerable<KeyValuePair<string, string>> formValues) {
		// Arrange
		var client = CreateClient();

		using var request = new HttpRequestMessage(HttpMethod.Post, "api/collection/int") {
			Content = new FormUrlEncodedContent(formValues),
		};

		// Act
		using var response = await client.SendAsync(request);
		var json = await response.Content.ReadAsStringAsync();
		var values = JsonSerializer.Deserialize<IEnumerable<int>>(json, _jsonSerializerOptions);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(values);
		Assert.Equal(new[] { 1, 2 }, values);
	}

	[Theory(DisplayName = "intに変換できない文字列やnullが含まれている場合は、IEnumerable<int>型のvaluesにバインドできずバリデーションエラーになる")]
	[InlineData("")]
	[InlineData("a")]
	[InlineData(null)]
	public async Task PostAsync_CanNotBindToInt32Values(string? value) {
		// Arrange
		var client = CreateClient();

		var formValues = new Dictionary<string, string?> {
			{ "values[0]", "1" },
			{ "values[1]", value },
		};

		using var request = new HttpRequestMessage(HttpMethod.Post, "api/collection/int") {
			Content = new FormUrlEncodedContent(formValues),
		};

		// Act
		using var response = await client.SendAsync(request);
		var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

		// Assert
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		Assert.NotNull(problem);
		Assert.Collection(
			problem.Errors.OrderBy(error => error.Key),
			entry => {
				Assert.Equal("values[1]", entry.Key);
				foreach (var message in entry.Value) {
					WriteLine(message);
				}
			});
	}

	[Theory(DisplayName = "空文字やnullもIEnumerable<string>型のvaluesにバインドできるがnullとして扱われる")]
	[InlineData("")]
	[InlineData(null)]
	public async Task PostAsync_CanBindToStringValues(string? value) {
		// Arrange
		var client = CreateClient();

		var formValues = new Dictionary<string, string?> {
			{ "values[0]", "a" },
			{ "values[1]", value },
		};

		using var request = new HttpRequestMessage(HttpMethod.Post, "api/collection/string") {
			Content = new FormUrlEncodedContent(formValues),
		};

		// Act
		using var response = await client.SendAsync(request);
		var json = await response.Content.ReadAsStringAsync();
		var values = JsonSerializer.Deserialize<IEnumerable<string>>(json, _jsonSerializerOptions);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(values);
		// 空文字もnullとして扱われる
		Assert.Equal(new[] { "a", null }, values);
	}

	public static TheoryData<IEnumerable<KeyValuePair<string, string>>> GetTheoryData_ComplexValues() {
		return new() {
			new Dictionary<string, string>() {
				{ "values[0].Id", "1" },
				{ "values[0].Name", "a" },
				{ "values[1].Id", "2" },
				{ "values[1].Name", "b" },
			},
		};
	}

	[Theory(DisplayName = "IEnumerable<Sample>型のvaluesにバインドできる")]
	[MemberData(nameof(GetTheoryData_ComplexValues))]
	public async Task PostAsync_CanBindToComplexModels(IEnumerable<KeyValuePair<string, string>> formValues) {
		// Arrange
		var client = CreateClient();

		using var request = new HttpRequestMessage(HttpMethod.Post, "api/collection/complex") {
			Content = new FormUrlEncodedContent(formValues),
		};

		// Act
		using var response = await client.SendAsync(request);
		var json = await response.Content.ReadAsStringAsync();
		var values = JsonSerializer.Deserialize<IEnumerable<Sample>>(json, _jsonSerializerOptions);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.NotNull(values);
		// .NET 8
		// Sample[]ではなくList<Sample>になった
		//Assert.Equal(new[] { new Sample { Id = 1, Name = "a" }, new Sample { Id = 2, Name = "b" } }, values);
		Assert.Collection(
			values.OrderBy(sample => sample.Id),
			sample => {
				Assert.Equal(1, sample.Id);
				Assert.Equal("a", sample.Name);
			},
			sample => {
				Assert.Equal(2, sample.Id);
				Assert.Equal("b", sample.Name);
			});
	}
}
