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

		// 1をキャンセルする
		tokenSource1.Cancel();

		// Assert
		Assert.True(token1.IsCancellationRequested);
		Assert.True(token2.IsCancellationRequested);
	}

	[Fact]
	public void CreateLinkedTokenSource_複数のトークンに紐付けてソースを作成して動きを確認する() {
		// Arrange
		// Act
		var tokenSource1 = new CancellationTokenSource();
		var tokenSource2 = new CancellationTokenSource();
		var tokenSource3 = new CancellationTokenSource();
		var tokenSourceLinked = CancellationTokenSource.CreateLinkedTokenSource(tokenSource1.Token, tokenSource2.Token, tokenSource3.Token);

		var token1 = tokenSource1.Token;
		var token2 = tokenSource2.Token;
		var token3 = tokenSource3.Token;
		var tokenLinked = tokenSourceLinked.Token;

		// 2をキャンセルする
		tokenSource2.Cancel();

		// Assert
		// 2はキャンセルした
		Assert.True(token2.IsCancellationRequested);
		// リンクされていたのでキャンセルされた
		Assert.True(tokenLinked.IsCancellationRequested);
		// 1と3は2のキャンセルに関係ない
		Assert.False(token1.IsCancellationRequested);
		Assert.False(token3.IsCancellationRequested);
	}
}
