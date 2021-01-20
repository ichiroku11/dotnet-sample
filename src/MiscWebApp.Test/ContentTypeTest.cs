using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApp.Test {
	public class ContentTypeTest : TestBase {
		public ContentTypeTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
			: base(output, factory) {
		}

		[Theory]
		[InlineData(".txt", "text/plain")]
		[InlineData("test.html", "text/html")]
		public async Task 期待するコンテンツタイプを取得できる(string subpath, string expected) {
			// Arrange
			var request = new HttpRequestMessage(HttpMethod.Get, $"/contenttype/{subpath}");

			// Act
			var response = await SendAsync(request);
			var actual = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData(".xyz")]
		public async Task コンテンツタイプが見つからない(string subpath) {
			// Arrange
			var request = new HttpRequestMessage(HttpMethod.Get, $"/contenttype/{subpath}");

			// Act
			var response = await SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
			Assert.Empty(content);
		}
	}
}
