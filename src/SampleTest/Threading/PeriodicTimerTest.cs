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
}
