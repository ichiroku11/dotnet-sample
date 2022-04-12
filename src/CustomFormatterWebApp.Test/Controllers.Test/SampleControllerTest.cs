using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CustomFormatterWebApp.Controllers.Test {
	// https://docs.microsoft.com/ja-jp/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
	public class SampleControllerTest : IClassFixture<WebApplicationFactory<Program>> {
		[Fact]
		public void Test1() {
			Assert.False(true);
		}
	}
}
