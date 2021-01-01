using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test {
	public class EnumControllerTest : ControllerTestBase {
		public EnumControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
			: base(output, factory) {
		}

		[Theory]
		[InlineData("1")]
		[InlineData("apple")]
		[InlineData("Apple")]
		public async Task Get_Enumにバインドできる(string fruit) {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/enum/{fruit}");

			// Act
			using var response = await SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal("1", content);
		}
	}
}
