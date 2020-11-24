using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Threading {
	public class CancellationTokenSourceTest : IDisposable {
		private readonly ITestOutputHelper _output;
		private readonly CancellationTokenSource _source;


		public CancellationTokenSourceTest(ITestOutputHelper output) {
			_output = output;
			_source = new CancellationTokenSource();
		}

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
	}
}
