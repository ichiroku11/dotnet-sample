using Polly;
using Xunit.Sdk;

namespace SampleTest.Polly;

public class PolicyHandleInnerTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class SampleException : Exception {
	}

	public static TheoryData<Exception> GetTheoryData_HandleInner_NotRetry() {
		return new() {
			// 内部例外がnullだとリトライされない
			new Exception("", null),

			// 内部例外が対象の例外ではないとリトライされない
			new Exception("", new ArgumentOutOfRangeException()),
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_HandleInner_NotRetry))]
	public void HandleInner_内部の例外が対象の例外と一致しない場合はリトライされない(Exception exception) {
		// Arrange
		var policy = Policy.HandleInner<SampleException>().Retry();
		var executedCount = 0;

		// Act
		Assert.Throws<Exception>(() => {
			policy.Execute(() => {
				executedCount++;
				_output.WriteLine($"Executed: {executedCount}");

				throw exception;
			});
		});

		// Assert
		// リトライされない
		Assert.Equal(1, executedCount);
	}

	public static TheoryData<Exception> GetTheoryData_HandleInner_Retry() {
		return new() {
			// 内部例外が対象の例外だとリトライされる
			// 発生する例外はExceptionではなくSampleExceptionになる様子
			new Exception(null, new SampleException()),

			// 外側の例外が対象の例外だとリトライされる
			new SampleException(),
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_HandleInner_Retry))]
	public void HandleInner_対象の例外と一致する場合はリトライする(Exception exception) {
		// Arrange
		var policy = Policy.HandleInner<SampleException>().Retry();
		var executedCount = 0;

		// Act
		Assert.Throws<SampleException>(() => {
			policy.Execute(() => {
				executedCount++;
				_output.WriteLine($"Executed: {executedCount}");

				throw exception;
			});
		});

		// Assert
		// リトライされている
		Assert.Equal(2, executedCount);
	}
}
