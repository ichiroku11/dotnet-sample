using Microsoft.AspNetCore.Http;

namespace SampleTest.AspNetCore.Http.HttpResults;

public class OkOfTValueTest {
	[Fact]
	public void Value_生成されるインスタンスを確認する() {
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

	// todo: result.ExecuteAsync
}
