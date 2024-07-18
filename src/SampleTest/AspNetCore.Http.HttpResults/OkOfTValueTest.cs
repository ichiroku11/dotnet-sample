using Microsoft.AspNetCore.Http;

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
	public async Task ExecuteAsync_レスポンスを確認する_空のオブジェクト() {
		// Arrange
		var value = new { };
		var result = TypedResults.Ok(value);

		var context = HttpContextHelper.CreateWithResponseBody();

		// Act
		await result.ExecuteAsync(context);

		// Assert
		Assert.Equal(200, context.Response.StatusCode);
		Assert.Equal("application/json; charset=utf-8", context.Response.ContentType);

		var responseBody = await context.Response.ReadBodyAsString();
		Assert.Equal("{}", responseBody);
	}

	[Fact]
	public async Task ExecuteAsync_レスポンスを確認する_プロパティを持つオブジェクト() {
		// Arrange
		var value = new { x = 1, y = "abc" };
		var result = TypedResults.Ok(value);

		var context = HttpContextHelper.CreateWithResponseBody();

		// Act
		await result.ExecuteAsync(context);

		// Assert
		Assert.Equal(200, context.Response.StatusCode);
		Assert.Equal("application/json; charset=utf-8", context.Response.ContentType);

		var responseBody = await context.Response.ReadBodyAsString();
		Assert.Equal(@"{""x"":1,""y"":""abc""}", responseBody);
	}
}
