
using Polly;

namespace SampleTest.Polly;

public class PolicyBuilderOrResultTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	// HandleResultで指定した条件なのでリトライされる
	[InlineData(1, 2)]
	// OrResultで指定した条件なのでリトライされる
	[InlineData(2, 2)]
	// 指定した条件ではないのでリトライされない
	[InlineData(3, 1)]
	public void OrResult_結果に基づいてリトライされる(int result, int expectedCount) {
		// Arrange
		var policy = Policy
			.HandleResult<int>(value => value == 1)
			.OrResult(value => value == 2)
			.Retry();
		var actualCount = 0;

		// Act
		policy.Execute(() => {
			actualCount++;
			_output.WriteLine($"Executed: {actualCount}");

			return result;
		});

		// Assert
		Assert.Equal(expectedCount, actualCount);
	}
}
