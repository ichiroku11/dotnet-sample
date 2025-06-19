namespace SampleTest.Threading;

public class TaskTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public async Task Delay_マイナスの値を指定すると例外が発生する() {
		// Arrange
		// Act
		var actual = await Record.ExceptionAsync(async () => {
			await Task.Delay(-2);
			// -1だと無限に待機する様子
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(actual);
		_output.WriteLine(actual.Message);
		// The value needs to be either -1 (signifying an infinite timeout), 0 or a positive integer. (Parameter 'millisecondsDelay')
	}

	[Fact]
	public async Task Delay_マイナスのTimeSpan値を指定すると例外が発生する() {
		// Arrange
		// Act
		var actual = await Record.ExceptionAsync(async () => {
			await Task.Delay(TimeSpan.FromMilliseconds(-2L));
			// -1Lだと無限に待機する様子
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(actual);
		_output.WriteLine(actual.Message);
		// The value needs to translate in milliseconds to -1 (signifying an infinite timeout), 0, or a positive integer less than or equal to the maximum allowed timer duration. (Parameter 'delay')
	}

	// todo: use Record
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
