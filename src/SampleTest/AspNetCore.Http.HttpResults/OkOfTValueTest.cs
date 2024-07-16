using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace SampleTest.AspNetCore.Http.HttpResults;

public class OkOfTValueTest {
	[Fact]
	public void Properties_インスタンスのプロパティを確認する() {
		// Arrange
		var value = new { };

		// Act
		var result = TypedResults.Ok(value);

		// Assert
		// Ok<TValue>型が生成される
		Assert.True(result.GetType().IsGenericType);
		Assert.Equal(200, result.StatusCode);
		Assert.Same(value, result.Value);
	}

	[Fact]
	public async Task ExecuteAsync_レスポンスを確認する() {
		// Arrange
		var value = new {};
		var result = TypedResults.Ok(value);

		var services = new ServiceCollection();
		services.AddSingleton<ILoggerFactory, NullLoggerFactory>();

		using var responseBodyStream = new MemoryStream();
		var context = new DefaultHttpContext {
			RequestServices = services.BuildServiceProvider(),
		};
		context.Response.Body = responseBodyStream;

		// Act
		await result.ExecuteAsync(context);

		// Assert
		Assert.Equal(200, context.Response.StatusCode);
		Assert.Equal("application/json; charset=utf-8", context.Response.ContentType);

		responseBodyStream.Position = 0;
		using var responseBodyReader = new StreamReader(responseBodyStream);
		var responseBody = await responseBodyReader.ReadToEndAsync();
		Assert.Equal("{}", responseBody);
	}
}
