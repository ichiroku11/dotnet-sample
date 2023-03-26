using Polly;

namespace SampleTest.Polly;

public class RetryPolicyTest {
	private class SampleException : Exception {
	}

	[Fact]
	public void Execute_例外が発生するとリトライされる() {
		// Arrange
		// Retryの引数を省略すると1回リトライする
		var policy = Policy.Handle<SampleException>().Retry();
		var count = 0;

		// Act
		// 例外が発生するので1回リトライされる
		// リトライされた後も例外が発生するので、Executeは例外が発生する
		Assert.Throws<SampleException>(() => {
			policy.Execute(() => {
				count++;
				throw new SampleException();
			});
		});

		// Assert
		Assert.Equal(2, count);
	}

	[Fact]
	public void Execute_例外が発生しないとリトライされない() {
		// Arrange
		// Retryの引数を省略すると1回リトライする
		var policy = Policy.Handle<SampleException>().Retry();
		var count = 0;

		// Act
		// 例外が発生しないのでリトライされない
		var result = policy.Execute(() => {
			count++;
			return -1;
		});

		// Assert
		// 1度実行される
		Assert.Equal(1, count);
		Assert.Equal(-1, result);
	}

	[Fact]
	public void Execute_1回リトライすると成功する動きを確認する() {
		// Arrange
		// Retryの引数を省略すると1回リトライする
		var policy = Policy.Handle<SampleException>().Retry();
		var count = 0;

		// Act
		// 1回リトライすると成功する
		var result = policy.Execute(() => {
			count++;
			if (count == 1) {
				throw new SampleException();
			}

			return count;
		});

		// Assert
		Assert.Equal(2, count);
	}
}
