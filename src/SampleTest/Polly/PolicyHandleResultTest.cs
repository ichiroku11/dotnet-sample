using Polly;

namespace SampleTest.Polly;

public class PolicyHandleResultTest {
	private readonly ITestOutputHelper _output;

	public PolicyHandleResultTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void HandleResult_結果に基づいて1回リトライする動きを確認する() {
		// Arrange
		// 結果が負の数であれば2回までリトライする
		var policy = Policy.HandleResult<int>(value => value < 0).Retry(2);
		var executedCount = 0;

		// Act
		var result = policy.Execute(() => {
			executedCount++;
			_output.WriteLine($"Executed: {executedCount}");

			// 1回目はリトライされる
			if (executedCount == 1) {
				return -1;
			}

			// 2回目以降は成功するのでリトライしない
			return 1;
		});

		// Assert
		Assert.Equal(2, executedCount);
		Assert.Equal(1, result);
	}

	[Fact]
	public void HandleResult_結果に基づいて2回リトライする動きを確認する() {
		// Arrange
		// 結果が負の数であれば2回までリトライする
		var policy = Policy.HandleResult<int>(value => value < 0).Retry(2);
		var executedCount = 0;

		// Act
		var result = policy.Execute(() => {
			executedCount++;
			_output.WriteLine($"Executed: {executedCount}");

			// 必ずリトライされるようにしてみる
			return -1;
		});

		// Assert
		Assert.Equal(3, executedCount);
		Assert.Equal(-1, result);
	}
}
