using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;

namespace SampleTest.AspNetCore.Http.HttpResults;

public class ResultsTest {
	[Fact]
	public void Okを保持していることを確認する() {
		// Arrange
		var expected = TypedResults.Ok();

		// Act
		// OkかBadRequestのどちらかを保持できる結果
		// 今回はOkを保持している
		Results<Ok, BadRequest> results = expected;

		// Assert
		Assert.IsType<Results<Ok, BadRequest>>(results);
		Assert.Same(expected, results.Result);
	}

	[Fact]
	public void BadRequestを保持していることを確認する() {
		// Arrange
		var expected = TypedResults.BadRequest();

		// Act
		// OkかBadRequestのどちらかを保持できる結果
		// 今回はBadRequestを保持している
		Results<Ok, BadRequest> results = expected;

		// Assert
		Assert.IsType<Results<Ok, BadRequest>>(results);
		Assert.Same(expected, results.Result);
	}
}
