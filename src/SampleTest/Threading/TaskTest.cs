namespace SampleTest.Threading;

public class TaskTest {
	[Fact]
	public async Task FromCanceled_キャンセルされたCancellationTokenを渡す() {
		// Arrange
		// Act
		// キャンセルされたCancellationTokenを渡す必要がある
		var task = Task.FromCanceled(new CancellationToken(true));

		// Assert
		await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
	}

	[Fact]
	public async Task FromCanceled_キャンセルされていないCancellationTokenを渡すとArgumentOutOfRangeException() {
		// Arrange
		// Act
		// Assert
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => {
			// キャンセルされていないCancellationTokenを渡すと例外
			return Task.FromCanceled(new CancellationToken(false));
		});
	}

	[Fact]
	public async Task FromException_引数に指定した例外が発生するタスクを作る() {
		// Arrange
		// Act
		var task = Task.FromException(new ArgumentException());

		// Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await task);
	}

	[Fact]
	public void Wait_例外はAggregateExceptionに集約される() {
		// Arrange
		// Act
		var task = Task.FromCanceled(new CancellationToken(true));

		// Assert
		var exception = Assert.Throws<AggregateException>(() => task.Wait());
		Assert.Single(exception.InnerExceptions);
		Assert.IsType<TaskCanceledException>(exception.InnerException);
	}
}
