using Polly;

namespace SampleTest.Polly;

public class RetryPolicyTest {
	private readonly ITestOutputHelper _output;

	public RetryPolicyTest(ITestOutputHelper output) {
		_output = output;
	}

	private class SampleException : Exception {
	}

	[Fact]
	public void Handle_例外が発生するとリトライされる() {
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
				_output.WriteLine($"Executed: {count}");

				throw new SampleException();
			});
		});

		// Assert
		Assert.Equal(2, count);
	}

	[Fact]
	public void Handle_例外が発生しないとリトライされない() {
		// Arrange
		var policy = Policy.Handle<SampleException>().Retry();
		var count = 0;

		// Act
		// 例外が発生しないのでリトライされない
		var result = policy.Execute(() => {
			count++;
			_output.WriteLine($"Executed: {count}");

			return -1;
		});

		// Assert
		// 1度実行される
		Assert.Equal(1, count);
		Assert.Equal(-1, result);
	}

	[Fact]
	public void Handle_例外が発生して1回リトライすると成功する動きを確認する() {
		// Arrange
		var policy = Policy.Handle<SampleException>().Retry();
		var count = 0;

		// Act
		// 1回リトライすると成功する
		var result = policy.Execute(() => {
			count++;
			_output.WriteLine($"Executed: {count}");

			if (count == 1) {
				throw new SampleException();
			}

			return -1;
		});

		// Assert
		Assert.Equal(2, count);
		Assert.Equal(-1, result);
	}

	[Fact]
	public void HandleResult_結果に基づいて1回リトライする動きを確認する() {
		// Arrange
		// 結果が負の数であれば2回までリトライする
		var policy = Policy.HandleResult<int>(value => value < 0).Retry(2);
		var count = 0;

		// Act
		var result = policy.Execute(() => {
			count++;
			_output.WriteLine($"Executed: {count}");

			// 1回目はリトライされる
			if (count == 1) {
				return -1;
			}

			// 2回目以降は成功するのでリトライしない
			return 1;
		});

		// Assert
		Assert.Equal(2, count);
		Assert.Equal(1, result);
	}

	[Fact]
	public void HandleResult_結果に基づいて2回リトライする動きを確認する() {
		// Arrange
		// 結果が負の数であれば2回までリトライする
		var policy = Policy.HandleResult<int>(value => value < 0).Retry(2);
		var count = 0;

		// Act
		var result = policy.Execute(() => {
			count++;
			_output.WriteLine($"Executed: {count}");

			// 必ずリトライされるようにしてみる
			return -1;
		});

		// Assert
		Assert.Equal(3, count);
		Assert.Equal(-1, result);
	}

	public static TheoryData<Exception?> GetTheoryData_HandleInner_NotRetry() {
		return new() {
			// 内部例外はnull
			new Exception("", null),
			// 内部の例外が対象の例外ではない
			new Exception("", new ArgumentOutOfRangeException()),
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_HandleInner_NotRetry))]
	public void HandleInner_内部の例外が対象の例外と一致しない場合はリトライされない(Exception exception) {
		// Arrange
		var policy = Policy.HandleInner<SampleException>().Retry();
		var count = 0;

		// Act
		Assert.Throws<Exception>(() => {
			policy.Execute(() => {
				count++;
				throw exception;
			});
		});

		// Assert
		// リトライされない
		Assert.Equal(1, count);
	}
}
