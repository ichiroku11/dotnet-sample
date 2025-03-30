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
		var options = GetAntiforgeryOptions(context);

		// Act
		var actual = antiforgery.GetTokens(context);

		// Assert
		Assert.NotNull(actual.RequestToken);
		Assert.NotNull(actual.CookieToken);
		Assert.Equal(options.FormFieldName, actual.FormFieldName);
		Assert.Equal(options.HeaderName, actual.HeaderName);
	}

	[Fact]
	public void GetTokens_ResponseにCookieが設定されない() {
		// Arrange
		var context = CreateHttpContext();
		var antiforgery = GetAntiforgery(context);

		// Act
		var _ = antiforgery.GetTokens(context);
		var headers = context.Response.GetTypedHeaders();

		// Assert
		// Cookieは設定されない
		Assert.Empty(headers.SetCookie);
	}

	[Fact]
	public void GetAndStoreTokens_戻り値を確認する() {
		// Arrange
		var context = CreateHttpContext();
		var antiforgery = GetAntiforgery(context);
		var options = GetAntiforgeryOptions(context);

		// Act
		var actual = antiforgery.GetAndStoreTokens(context);

		// Assert
		Assert.NotNull(actual.RequestToken);
		Assert.NotNull(actual.CookieToken);
		Assert.Equal(options.FormFieldName, actual.FormFieldName);
		Assert.Equal(options.HeaderName, actual.HeaderName);
	}

	[Fact]
	public void GetAndStoreTokens_ResponseにCookieが設定される() {
		// Arrange
		var context = CreateHttpContext();
		var antiforgery = GetAntiforgery(context);
		var options = GetAntiforgeryOptions(context);

		// Act
		var _ = antiforgery.GetAndStoreTokens(context);
		var headers = context.Response.GetTypedHeaders();

		// Assert
		// Cookieが1つ設定されている
		var headerValue = Assert.Single(headers.SetCookie);
		_output.WriteLine(headerValue.ToString());

		// AntiforgeryOptionsのクッキー設定が反映されている
		Assert.Equal(options.Cookie.Name, headerValue.Name);
		Assert.Equal(options.Cookie.HttpOnly, headerValue.HttpOnly);
		Assert.Equal(options.Cookie.Domain, headerValue.Domain);

		Assert.Equal("/", headerValue.Path);

		// 名前空間が異なるため数値で比較する
		// https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.http.samesitemode?view=aspnetcore-9.0
		// https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.net.http.headers.samesitemode?view=aspnetcore-9.0
		Assert.Equal((int)options.Cookie.SameSite, (int)headerValue.SameSite);
	}

	[Fact]
	public async Task IsRequestValidAsync_GETメソッドではtrueを返す() {
		// Arrange
		var context = CreateHttpContext();
		// GET以外にHEAD、OPTIONS、TRACEもtrueを返す様子
		context.Request.Method = HttpMethods.Get;

		var antiforgery = GetAntiforgery(context);

		// Act
		var actual = await antiforgery.IsRequestValidAsync(context);

		// Assert
		Assert.True(actual);
	}

	[Fact]
	public async Task IsRequestValidAsync_POSTメソッドではfalseを返す() {
		// Arrange
		// Cookie、ヘッダーやPOSTデータが存在しない場合
		var context = CreateHttpContext();
		context.Request.Method = HttpMethods.Post;

		var antiforgery = GetAntiforgery(context);

		// Act
		var actual = await antiforgery.IsRequestValidAsync(context);

		// Assert
		Assert.False(actual);
	}
}
