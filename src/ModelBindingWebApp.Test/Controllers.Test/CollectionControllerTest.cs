using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test {
	public class CollectionControllerTest : ControllerTestBase {
		private static readonly JsonSerializerOptions _jsonSerializerOptions
			= new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};


		public CollectionControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
			: base(output, factory) {
		}

		public static IEnumerable<object[]> GetSimpleValues() {
			// values=1&values=2
			yield return new object[] {
				new[] {
					KeyValuePair.Create("values", "1"),
					KeyValuePair.Create("values", "2"),
				},
			};

			// values[0]=1&values[1]=2
			yield return new object[] {
				new Dictionary<string, string>() {
					{ "values[0]", "1" },
					{ "values[1]", "2" },
				},
			};

			// values[0]=1&values[1]=2&values[3]=3
			yield return new object[] {
				new Dictionary<string, string>() {
					{ "values[0]", "1" },
					{ "values[1]", "2" },
					// 欠番以降は無視される
					{ "values[3]", "3" },
				},
			};

			// [0]=1&[1]=2
			yield return new object[] {
				new Dictionary<string, string>() {
					{ "[0]", "1" },
					{ "[1]", "2" },
				},
			};

			// values[]=1&values[]=2
			yield return new object[] {
				new[] {
					KeyValuePair.Create("values[]", "1"),
					KeyValuePair.Create("values[]", "2"),
				},
			};
		}

		[Theory(DisplayName = "IEnumerable<int>型のvaluesにバインドできる")]
		[MemberData(nameof(GetSimpleValues))]
		public async Task PostAsync_BindToInt32Enumerable(IEnumerable<KeyValuePair<string, string>> formValues) {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Post, "api/collection") {
				Content = new FormUrlEncodedContent(formValues),
			};

			// Act
			using var response = await SendAsync(request);
			var json = await response.Content.ReadAsStringAsync();
			var values = JsonSerializer.Deserialize<IEnumerable<int>>(json, _jsonSerializerOptions);

			// Assert
			Assert.Equal(new[] { 1, 2 }, values);
		}

		public static IEnumerable<object[]> GetComplexValues() {
			yield return new object[] {
				new Dictionary<string, string>() {
					{ "values[0].Id", "1" },
					{ "values[0].Name", "a" },
					{ "values[1].Id", "2" },
					{ "values[1].Name", "b" },
				},
			};
		}

		[Theory(DisplayName = "IEnumerable<Sample>型のvaluesにバインドできる")]
		[MemberData(nameof(GetComplexValues))]
		public async Task PostAsync_BindToComplexModelEnumerable(IEnumerable<KeyValuePair<string, string>> formValues) {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Post, "api/collection/complex") {
				Content = new FormUrlEncodedContent(formValues),
			};

			// Act
			using var response = await SendAsync(request);
			var json = await response.Content.ReadAsStringAsync();
			var values = JsonSerializer.Deserialize<IEnumerable<Sample>>(json, _jsonSerializerOptions);

			// Assert
			Assert.Equal(new[] { new Sample { Id = 1, Name = "a" }, new Sample { Id = 2, Name = "b" } }, values);
		}
	}
}
