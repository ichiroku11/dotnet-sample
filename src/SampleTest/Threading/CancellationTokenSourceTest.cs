namespace SampleTest.Threading;

public class CancellationTokenSourceTest(ITestOutputHelper output) : IDisposable {
	private readonly ITestOutputHelper _output = output;
	private readonly CancellationTokenSource _source = new CancellationTokenSource();

	public void Dispose() {
		_source?.Dispose();
	}

	// 参考
	// https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.cancellationtokensource.cancel?view=netframework-4.8
	[Fact]
	public async Task Cancel_awaitするとTaskCanceledExceptionがスローされる() {
		// Arrange
		// Act

		// 無限に待機するタスク
		var task = Task.Delay(Timeout.Infinite, _source.Token);

		// トークンソースでキャンセルする
		_source.Cancel();

		// Assert
		await Assert.ThrowsAsync<TaskCanceledException>(async () => {
			// タスクの完了、キャンセルを待機
			await task;
		});
	}

	[Fact]
	public async Task Cancel_キャンセルするとRegisterで登録したコールバックが呼ばれる() {
		// Arrange
		// Act
		// Assert
		var token = _source.Token;

		// キャンセル時のコールバックを指定する
		var callback = false;
		token.Register(() => {
			_output.WriteLine("canceled");
			callback = true;
		});

		var task = Task.Delay(Timeout.Infinite, token);

		// コールバックはまだ呼ばれていない
		Assert.False(callback);

		// キャンセル
		_source.Cancel();

		// コールバックが呼ばれた
		Assert.True(callback);

		await Assert.ThrowsAsync<TaskCanceledException>(async () => {
			await task;
		});
	}

	// 完璧なテストじゃないかも
	/*
	[Fact]
	public async Task CancelAfter_指定ms後にキャンセルする() {
		// Arrange
		// Act
		// Assert
		var token = _source.Token;

		// キャンセル時のコールバックを指定する
		var callback = false;
		token.Register(() => {
			_output.WriteLine("canceled");
			callback = true;
		});

		var task = Task.Delay(Timeout.Infinite, token);

		var timeout = 1000;

		// 100ms後にキャンセルする
		_source.CancelAfter(timeout);

		// コールバックはまだ呼ばれていない
		Assert.False(callback);

		await Task.Delay(timeout);

		// コールバックが呼ばれた
		Assert.True(callback);

		await Assert.ThrowsAsync<TaskCanceledException>(async () => {
			await task;
		});
	}
	*/

	[Fact]
	public void CreateLinkedTokenSource_動きを確認する() {
		// Arrange
		// Act
		var tokenSource1 = new CancellationTokenSource();
		var tokenSource2 = CancellationTokenSource.CreateLinkedTokenSource(tokenSource1.Token);
		var token1 = tokenSource1.Token;
		var token2 = tokenSource2.Token;

		// 1つめをキャンセルする
		tokenSource1.Cancel();

		// Assert
		Assert.True(token1.IsCancellationRequested);
		Assert.True(token2.IsCancellationRequested);
	}
}
