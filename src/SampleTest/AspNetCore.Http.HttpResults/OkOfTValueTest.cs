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
		var value = new { };
		var result = TypedResults.Ok(value);

		var services = new ServiceCollection();
		services.AddSingleton<ILoggerFactory, NullLoggerFactory>();

		var context = new DefaultHttpContext {
			RequestServices = services.BuildServiceProvider(),
		};

		// Act
		await result.ExecuteAsync(context);

		// Assert
		Assert.Equal(200, context.Response.StatusCode);
		// todo:
		Assert.Same(Stream.Null, context.Response.Body);
	}
}
