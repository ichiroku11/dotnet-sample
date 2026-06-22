using Microsoft.AspNetCore.Http;

namespace SampleTest.AspNetCore.Http;

public class TypedResultsTest {
	[Fact]
	public void Ok_生成されるインスタンスのステータスコードを確認する() {
		// Arrange

		// Act
		var result = TypedResults.Ok();

		// Assert
		Assert.Equal(200, result.StatusCode);
	}

	[Fact]
	public void NoContent_生成されるインスタンスのステータスコードを確認する() {
		// Arrange

		// Act
		var result = TypedResults.NoContent();

		// Assert
		Assert.Equal(204, result.StatusCode);
	}
}
