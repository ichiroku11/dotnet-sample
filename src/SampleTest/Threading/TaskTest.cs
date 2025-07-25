namespace SampleTest.Threading;

public class TaskTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	// -2ms以下の値を指定すると例外が発生する
	// -1msだと無限に待機する様子
	[Theory]
	[InlineData(-2)]
	public async Task Delay_マイナスの値を指定すると例外が発生する(int milliseconds) {
		// Arrange
		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await Task.Delay(millisecondsDelay: milliseconds);
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
		_output.WriteLine(exception.Message);
		// The value needs to be either -1 (signifying an infinite timeout), 0 or a positive integer. (Parameter 'millisecondsDelay')
	}

	// -2ms以下の値を指定すると例外が発生する
	// doubleをlongに変換して-1msだと無限に待機する様子
	[Theory]
	[InlineData(-2.0)]
	public async Task Delay_マイナスのTimeSpanを指定すると例外が発生する(double milliseconds) {
		// Arrange
		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await Task.Delay(delay: TimeSpan.FromMilliseconds(milliseconds));
		});

		// Assert
		Assert.IsType<ArgumentOutOfRangeException>(exception);
		_output.WriteLine(exception.Message);
		// The value needs to translate in milliseconds to -1 (signifying an infinite timeout), 0, or a positive integer less than or equal to the maximum allowed timer duration. (Parameter 'delay')
	}

	// -1ms ~ 0の間は例外は発生しない
	[Theory]
	[InlineData(-1L * 10_000 + 1)]
	[InlineData(-1L)]
	[InlineData(0)]
	public async Task Delay_マイナスのTimeSpanを指定しても例外が発生しない(long ticks) {
		// Arrange
		// Act
		var actual = await Record.ExceptionAsync(async () => {
			await Task.Delay(TimeSpan.FromTicks(ticks));
		});

		// Assert
		Assert.Null(actual);
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
