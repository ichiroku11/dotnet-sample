namespace SampleTest.Threading;

public class TaskTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public async Task Delay_マイナスの値を指定すると例外が発生する() {
		// Arrange
		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await Task.Delay(-2);
			// -1だと無限に待機する様子
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
		_output.WriteLine(exception.Message);
		// The value needs to be either -1 (signifying an infinite timeout), 0 or a positive integer. (Parameter 'millisecondsDelay')
	}

	[Fact]
	public async Task Delay_マイナスのTimeSpan値を指定すると例外が発生する() {
		// Arrange
		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await Task.Delay(TimeSpan.FromMilliseconds(-2L));
			// -1Lだと無限に待機する様子
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
		_output.WriteLine(exception.Message);
		// The value needs to translate in milliseconds to -1 (signifying an infinite timeout), 0, or a positive integer less than or equal to the maximum allowed timer duration. (Parameter 'delay')
	}

	[Fact]
	public async Task FromCanceled_キャンセルされたCancellationTokenを渡す() {
		// Arrange
		// Act
		// キャンセルされたCancellationTokenを渡す必要がある
		var exception = await Record.ExceptionAsync(async () => {
			await Task.FromCanceled(new CancellationToken(true));
		});

		// Assert
		Assert.IsType<TaskCanceledException>(exception);
		_output.WriteLine(exception.Message);
		// A task was canceled.
	}

	[Fact]
	public void FromCanceled_キャンセルされていないCancellationTokenを渡すとArgumentOutOfRangeException() {
		// Arrange
		// Act
		var exception = Record.Exception(() => {
			// キャンセルされていないCancellationTokenを渡すと例外
			Task.FromCanceled(new CancellationToken(false));
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
		_output.WriteLine(exception.Message);
		// Specified argument was out of the range of valid values. (Parameter 'cancellationToken')
	}

	[Fact]
	public async Task FromException_引数に指定した例外が発生するタスクを作る() {
		// Arrange
		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await Task.FromException(new ArgumentException("テスト"));
		});

		// Assert
		Assert.IsType<ArgumentException>(exception);
		Assert.Equal("テスト", exception.Message);
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void Wait_例外はAggregateExceptionに集約される() {
		// Arrange
		// Act
		var exception = Record.Exception(() => {
			var task = Task.FromCanceled(new CancellationToken(true));
			task.Wait();
		});

		// Assert
		var aggregateException = Assert.IsType<AggregateException>(exception);
		Assert.Single(aggregateException.InnerExceptions);
		Assert.IsType<TaskCanceledException>(exception.InnerException);
	}
}
