using Polly;

namespace SampleTest.Polly;

public class PolicyHandleTest {
	private class SampleException : Exception {
	}

	private readonly ITestOutputHelper _output;

	public PolicyHandleTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Handle_例外が発生するとリトライされる() {
		// Arrange
		// Retryの引数を省略すると1回リトライする
		var policy = Policy.Handle<SampleException>().Retry();
		var executedCount = 0;

		// Act
		// 例外が発生するので1回リトライされる
		// リトライされた後も例外が発生するので、Executeは例外が発生する
		Assert.Throws<SampleException>(() => {
			policy.Execute(() => {
				executedCount++;
				_output.WriteLine($"Executed: {executedCount}");

				throw new SampleException();
			});
		});

		// Assert
		Assert.Equal(2, executedCount);
	}

	[Fact]
	public void Handle_例外が発生しないとリトライされない() {
		// Arrange
		var policy = Policy.Handle<SampleException>().Retry();
		var executedCount = 0;

		// Act
		// 例外が発生しないのでリトライされない
		var result = policy.Execute(() => {
			executedCount++;
			_output.WriteLine($"Executed: {executedCount}");

			return -1;
		});

		// Assert
		// 1度実行される
		Assert.Equal(1, executedCount);
		Assert.Equal(-1, result);
	}

	[Fact]
	public void Handle_例外が発生して1回リトライすると成功する動きを確認する() {
		// Arrange
		var policy = Policy.Handle<SampleException>().Retry();
		var executedCount = 0;

		// Act
		// 1回リトライすると成功する
		var result = policy.Execute(() => {
			executedCount++;
			_output.WriteLine($"Executed: {executedCount}");

			if (executedCount == 1) {
				throw new SampleException();
			}

			return -1;
		});

		// Assert
		Assert.Equal(2, executedCount);
		Assert.Equal(-1, result);
	}
}
