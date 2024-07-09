using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

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

	// todo: result.ExecuteAsync
}
