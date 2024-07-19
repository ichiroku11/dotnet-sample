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
}
