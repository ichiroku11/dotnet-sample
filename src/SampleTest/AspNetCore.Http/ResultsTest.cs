using Microsoft.AspNetCore.Http;

namespace SampleTest.AspNetCore.Http;

public class ResultsTest {
	[Fact]
	public void Ok_生成されるインスタンスの型を確認する() {
		// Arrange

		// Act
		var result = Results.Ok();

		// Assert
		Assert.IsType(TypedResults.Ok().GetType(), result);
	}
}
