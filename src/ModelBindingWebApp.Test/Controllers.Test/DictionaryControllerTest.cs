using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Models;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test;

public class DictionaryControllerTest : ControllerTestBase {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	public DictionaryControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
		: base(output, factory) {
	}

	public static IEnumerable<object[]> GetSimpleValues() {
		// values[a]=1&values[b]=2
		yield return new object[] {
				new Dictionary<string, string>() {
					{ "values[a]", "1" },
					{ "values[b]", "2" },
				},
			};

		// [a]=1&[b]=2
		yield return new object[] {
				new Dictionary<string, string>() {
					{ "[a]", "1" },
					{ "[b]", "2" },
				},
			};

		yield return new object[] {
				new Dictionary<string, string>() {
					{ "values[0].Key", "a" },
					{ "values[0].Value", "1" },
					{ "values[1].Key", "b" },
					{ "values[1].Value", "2" },
				},
			};

		yield return new object[] {
				new Dictionary<string, string>() {
					{ "[0].Key", "a" },
					{ "[0].Value", "1" },
					{ "[1].Key", "b" },
					{ "[1].Value", "2" },
				},
			};
	}

	[Theory(DisplayName = "IDictionary<string, int>型のvaluesにバインドできる")]
	[MemberData(nameof(GetSimpleValues))]
	public async Task PostAsync_BindToStringInt32Dictionary(IEnumerable<KeyValuePair<string, string>> formValues) {
		// Arrange
		using var request = new HttpRequestMessage(HttpMethod.Post, "api/dictionary") {
			Content = new FormUrlEncodedContent(formValues),
		};

		// Act
		using var response = await SendAsync(request);
		var json = await response.Content.ReadAsStringAsync();
		var values = JsonSerializer.Deserialize<IDictionary<string, int>>(json, _jsonSerializerOptions)!;

		// Assert
		Assert.Equal(2, values.Count);
		Assert.Equal(1, values["a"]);
		Assert.Equal(2, values["b"]);
	}

	// 参照
	// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/models/model-binding?view=aspnetcore-5.0#dictionaries
	public static IEnumerable<object[]> GetComplexValues() {
		yield return new object[] {
				new Dictionary<string, string>() {
					{ "values[1].Id", "1" },
					{ "values[1].Name", "a" },
					{ "values[2].Id", "2" },
					{ "values[2].Name", "b" },
				},
			};

		yield return new object[] {
				new Dictionary<string, string>() {
					{ "values[0].Key", "1" },
					{ "values[0].Value.Id", "1" },
					{ "values[0].Value.Name", "a" },
					{ "values[1].Key", "2" },
					{ "values[1].Value.Id", "2" },
					{ "values[1].Value.Name", "b" },
				},
			};
	}

	[Theory(DisplayName = "IDictionary<int, Sample>型のvaluesにバインドできる")]
	[MemberData(nameof(GetComplexValues))]
	public async Task PostAsync_BindToComplexModelDictionary(IEnumerable<KeyValuePair<string, string>> formValues) {
		// Arrange
		using var request = new HttpRequestMessage(HttpMethod.Post, "api/dictionary/complex") {
			Content = new FormUrlEncodedContent(formValues),
		};

		// Act
		using var response = await SendAsync(request);
		var json = await response.Content.ReadAsStringAsync();
		var values = JsonSerializer.Deserialize<IDictionary<string, Sample>>(json, _jsonSerializerOptions)!;

		// Assert
		Assert.Equal(2, values.Count);
		Assert.Equal(1, values["1"].Id);
		Assert.Equal("a", values["1"].Name);
		Assert.Equal(2, values["2"].Id);
		Assert.Equal("b", values["2"].Name);
	}
}
