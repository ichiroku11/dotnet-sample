using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SampleTest.AspNetCore.Http;

public class ResultsTest {
	[Fact]
	public void Ok_生成されるインスタンスの型を確認する() {
		// Arrange

		// Act
		var result = Results.Ok();

		// Assert
		Assert.IsType<Ok>(result);
	}

	[Fact]
	public void NoContent_生成されるインスタンスの型を確認する() {
		// Arrange

		// Act
		var result = Results.NoContent();

		// Assert
		Assert.IsType<NoContent>(result);
	}
}
