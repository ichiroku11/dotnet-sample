using Polly;

namespace SampleTest.Polly;

public class RetryPolicyTest {
	[Fact]
	public void Execute_使い方を確認する() {
		// Arrange
		var retry = Policy.Handle<Exception>().Retry();
		var count = 0;

		// Act
		// 例外が発生しないのでリトライされない
		var result = retry.Execute(() => {
			count++;
			return -1;
		});

		// Assert
		Assert.Equal(1, count);
		Assert.Equal(-1, result);
	}
}
