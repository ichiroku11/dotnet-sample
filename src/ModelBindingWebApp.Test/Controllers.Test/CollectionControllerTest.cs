using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Models;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class CollectionControllerTest : ControllerTestBase {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};


	public CollectionControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
		: base(output, factory) {
	}

	public static TheoryData<IEnumerable<KeyValuePair<string, string>>> GetTheoryData_SimpleValues() {
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
	[MemberData(nameof(GetTheoryData_SimpleValues))]
	public async Task PostAsync_BindToInt32Enumerable(IEnumerable<KeyValuePair<string, string>> formValues) {
		// Arrange
		var client = CreateClient();

		using var request = new HttpRequestMessage(HttpMethod.Post, "api/collection") {
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

	[Theory]
	[InlineData("")]
	[InlineData("a")]
	[InlineData(null)]
	public async Task PostAsync_intに変換できない文字列やnullが含まれている場合はバリデーションエラーになる(string? value) {
		// Arrange
		var client = CreateClient();

		var formValues = new Dictionary<string, string?> {
			{ "values[0]", "1" },
			{ "values[1]", value },
		};

		using var request = new HttpRequestMessage(HttpMethod.Post, "api/collection") {
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
	public async Task PostAsync_BindToComplexModelEnumerable(IEnumerable<KeyValuePair<string, string>> formValues) {
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
		Assert.Equal(new[] { new Sample { Id = 1, Name = "a" }, new Sample { Id = 2, Name = "b" } }, values);
	}
}
