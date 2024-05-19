namespace SampleTest.Threading;

public class PeriodicTimerTest {
	public static TheoryData<TimeSpan> GetTheoryData_Constructor() {
		return new() {
			TimeSpan.Zero,
			TimeSpan.FromMilliseconds(uint.MaxValue)
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Constructor))]
	public void Constructor_許容範囲外の数値をコンストラクターに渡すと例外が発生する(TimeSpan period) {
		// Arrange
		// Act
		var exception = Record.Exception(() => {
			var _ = new PeriodicTimer(period);
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
	}

	[Fact]
	public async Task WaitForNextTickAsync_戻り値はtrueになる() {
		// Arrange
		using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1));

		// Act
		var actual = await timer.WaitForNextTickAsync();

		// Assert
		Assert.True(actual);
	}

	[Fact]
	public async Task WaitForNextTickAsync_タイマーをDisposeすると戻り値はfalseになる() {
		// Arrange
		using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1));
		timer.Dispose();

		// Act
		var actual = await timer.WaitForNextTickAsync();

		// Assert
		Assert.False(actual);
	}

	[Fact]
	public async Task WaitForNextTickAsync_キャンセル済みのキャンセルトークンを渡して呼び出すとTaskCanceledExceptionが発生する() {
		// Arrange
		using var tokenSource = new CancellationTokenSource();
		tokenSource.Cancel();

		using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1));

		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await timer.WaitForNextTickAsync(tokenSource.Token);
		});

		// Assert
		Assert.IsType<TaskCanceledException>(exception);
	}
}
