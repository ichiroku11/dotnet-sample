using Polly;

namespace SampleTest.Polly;

public class RetryPolicyTest(ITestOutputHelper output) {

	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Retry_引数を省略すると1回リトライされる() {
		// Arrange
		var policy = Policy
			.HandleResult<int>(value => value < 0)
			.Retry();
		var retryCount = 0;

		// Act
		var result = policy.Execute(() => {
			retryCount++;
			_output.WriteLine($"Executed: {retryCount}");
			return -1;
		});

		// Assert
		// Executeメソッドは2回呼ばれる
		Assert.Equal(2, retryCount);

		Assert.Equal(-1, result);
	}

	[Fact]
	public void Retry_onRetryコールバックが1回呼ばれることを確認する() {
		// Arrange
		var retryCount = 0;
		var policy = Policy
			.HandleResult<int>(value => value < 0)
			.Retry(onRetry: (result, count) => {
				retryCount++;
				_output.WriteLine($"Executed: {result.Result}, {count}, {retryCount}");

				// Executeの結果
				Assert.Equal(-1, result.Result);

				// 1回リトライされる
				Assert.Equal(retryCount, count);
			});

		// Act
		var result = policy.Execute(() => -1);

		// Assert
		// 1回リトライされる
		Assert.Equal(1, retryCount);

		Assert.Equal(-1, result);
	}
}
