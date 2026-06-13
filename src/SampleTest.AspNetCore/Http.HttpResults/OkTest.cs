using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SampleTest.AspNetCore.Http.HttpResults;

public class OkTest {
	[Fact]
	public void Properties_インスタンスのプロパティを確認する() {
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

		var context = HttpContextHelper.CreateWithResponseBody();

		// Act
		await result.ExecuteAsync(context);

		// Assert
		Assert.Equal(200, context.Response.StatusCode);
		Assert.Null(context.Response.ContentType);

		var responseBody = await context.Response.ReadBodyAsString();
		Assert.Equal("", responseBody);
	}
}
