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
}
