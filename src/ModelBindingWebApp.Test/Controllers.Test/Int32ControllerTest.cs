using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test {
	public class Int32ControllerTest : ControllerTestBase {
		public Int32ControllerTest(
			ITestOutputHelper output,
			WebApplicationFactory<Startup> factory)
			: base(output, factory) {
		}

		[Fact]
		public async Task GetWithQuery_クエリ文字列を省略するとintは0になる() {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Get, "/int32/getwithquery");

			// Act
			using var response = await SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal("0", content);
		}

		[Fact]
		public async Task GetWithRoute_ルートのパラメータを省略するとNotFound() {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Get, "/int32/getwithroute");

			// Act
			using var response = await SendAsync(request);

			// Assert
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task Post_フォームデータを省略するとintは0になる() {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Post, "/int32/post");

			// Act
			using var response = await SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal("0", content);
		}
	}
}
