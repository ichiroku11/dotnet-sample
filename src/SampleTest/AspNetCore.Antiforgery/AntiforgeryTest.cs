using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SampleTest.AspNetCore.Antiforgery;

public class AntiforgeryTest {
	[Fact]
	public void GetTokens_戻り値を確認する() {
		// Arrange
		var services = new ServiceCollection();
		services.AddLogging();
		services.AddAntiforgery();

		var context = new DefaultHttpContext {
			RequestServices = services.BuildServiceProvider(),
		};

		var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

		// Act
		var actual = antiforgery.GetTokens(context);

		// Assert
		Assert.NotNull(actual.RequestToken);
		Assert.NotNull(actual.CookieToken);
	}
}
