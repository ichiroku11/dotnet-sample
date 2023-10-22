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

	// todo: キャンセル済みのキャンセルトークンを渡す
	// todo: コンストラクターの例外など
}
