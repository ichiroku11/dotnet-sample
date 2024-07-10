using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace SampleTest.AspNetCore.Http.HttpResults;

public class OkTest {
	[Fact]
	public void StatusCode_生成されるインスタンスを確認する() {
		// Arrange

		// Act
		var result = TypedResults.Ok();

		// Assert
		// ジェネリクスではないOk型が生成される
		Assert.IsType<Ok>(result);
		Assert.Equal(200, result.StatusCode);
	}

	[Fact]
	public async Task ExecuteAsync_レスポンスを確認する() {
		// Arrange
		var result = TypedResults.Ok();

		var services = new ServiceCollection();
		services.AddSingleton<ILoggerFactory, NullLoggerFactory>();

		var context = new DefaultHttpContext {
			RequestServices = services.BuildServiceProvider(),
		};

		// Act
		await result.ExecuteAsync(context);

		// Assert
		Assert.Equal(200, context.Response.StatusCode);
		Assert.Same(Stream.Null, context.Response.Body);
	}
}
