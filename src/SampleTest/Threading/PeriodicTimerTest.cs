namespace SampleTest.Threading;

public class PeriodicTimerTest {
	[Fact]
	public async void WaitForNextTickAsync_戻り値はtrueになる() {
		// Arrange
		using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1));

		// Act
		var actual = await timer.WaitForNextTickAsync();

		// Assert
		Assert.True(actual);
	}

	[Fact]
	public async void WaitForNextTickAsync_タイマーをDisposeすると戻り値はfalseになる() {
		// Arrange
		using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(1));
		timer.Dispose();

		// Act
		var actual = await timer.WaitForNextTickAsync();

		// Assert
		Assert.False(actual);
	}

	[Fact]
	public async void WaitForNextTickAsync_キャンセル済みのキャンセルトークンを渡して呼び出すとTaskCanceledExceptionが発生する() {
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

	// todo: コンストラクターの例外など
}
