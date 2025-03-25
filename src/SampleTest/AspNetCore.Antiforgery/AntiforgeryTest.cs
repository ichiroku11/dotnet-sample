using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.AspNetCore.Antiforgery;

public class AntiforgeryTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private static DefaultHttpContext CreateHttpContext() {
		var services = new ServiceCollection();
		services.AddLogging();
		services.AddAntiforgery();

		return new DefaultHttpContext {
			RequestServices = services.BuildServiceProvider(),
		};
	}

	private static IAntiforgery GetAntiforgery(HttpContext context)
		=> context.RequestServices.GetRequiredService<IAntiforgery>();

	private static AntiforgeryOptions GetAntiforgeryOptions(HttpContext context)
		=> context.RequestServices.GetRequiredService<IOptions<AntiforgeryOptions>>().Value;

	[Fact]
	public void GetTokens_戻り値を確認する() {
		// Arrange
		var context = CreateHttpContext();
		var antiforgery = GetAntiforgery(context);
		var antiforgeryOptions = GetAntiforgeryOptions(context);

		// Act
		var actual = antiforgery.GetTokens(context);

		// Assert
		Assert.NotNull(actual.RequestToken);
		Assert.NotNull(actual.CookieToken);
		Assert.Equal(antiforgeryOptions.FormFieldName, actual.FormFieldName);
		Assert.Equal(antiforgeryOptions.HeaderName, actual.HeaderName);
	}

	// todo: GetTokensでResponseにCookieが設定されない

	[Fact]
	public void GetAndStoreTokens_戻り値を確認する() {
		// Arrange
		var context = CreateHttpContext();
		var antiforgery = GetAntiforgery(context);
		var antiforgeryOptions = GetAntiforgeryOptions(context);

		// Act
		var actual = antiforgery.GetAndStoreTokens(context);

		// Assert
		Assert.NotNull(actual.RequestToken);
		Assert.NotNull(actual.CookieToken);
		Assert.Equal(antiforgeryOptions.FormFieldName, actual.FormFieldName);
		Assert.Equal(antiforgeryOptions.HeaderName, actual.HeaderName);
	}

	[Fact]
	public void GetAndStoreTokens_ResponseにCookieが設定される() {
		// Arrange
		var context = CreateHttpContext();
		var antiforgery = GetAntiforgery(context);

		// Act
		var _ = antiforgery.GetAndStoreTokens(context);
		var headers = context.Response.GetTypedHeaders();

		// Assert
		var headerValue = Assert.Single(headers.SetCookie);
		_output.WriteLine(headerValue.ToString());
	}
}
