using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SampleTest.AspNetCore.Http.HttpResults;

public class BadRequestOfTValueTest {
	[Fact]
	public void Properties_インスタンスのプロパティを確認する() {
		// Arrange
		var value = new { };

		// Act
		var result = TypedResults.BadRequest(value);

		// Assert
		// BadRequest<TValue>型が生成される
		Assert.True(result.GetType().IsGenericType);
		Assert.Equal(typeof(BadRequest<>), result.GetType().GetGenericTypeDefinition());
		Assert.Equal(400, result.StatusCode);
		Assert.Same(value, result.Value);
	}

	[Fact]
	public async Task ExecuteAsync_レスポンスを確認する() {
		// Arrange
		var value = new { x = 1, y = "abc" };
		var result = TypedResults.BadRequest(value);

		var context = HttpContextHelper.CreateWithResponseBody();

		// Act
		await result.ExecuteAsync(context);

		// Assert
		Assert.Equal(400, context.Response.StatusCode);
		Assert.Equal("application/json; charset=utf-8", context.Response.ContentType);

		var responseBody = await context.Response.ReadBodyAsString();
		Assert.Equal(@"{""x"":1,""y"":""abc""}", responseBody);
	}
}
