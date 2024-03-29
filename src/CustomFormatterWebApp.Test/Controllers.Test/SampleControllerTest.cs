using CustomFormatterWebApp.Helpers;
using CustomFormatterWebApp.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace CustomFormatterWebApp.Controllers.Test {
	// https://docs.microsoft.com/ja-jp/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
	public class SampleControllerTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>> {
		private readonly WebApplicationFactory<Program> _factory = factory;

		[Fact]
		public async Task PostAsync_POSTした値を取得できる() {
			// Arrange
			using var client = _factory.CreateClient();
			var expected = new Sample(1, "a");

			// Act
			using var response = await client.PostAsBase64JsonAsync("/api/sample", expected);
			var actual = await response.Content.ReadFromBase64JsonAsync<Sample>();

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
