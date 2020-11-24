using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Threading {
	// TaskCompletionSourceを使ったサンプル
	public class TaskCompletionSourceTest {
		[Fact]
		public async Task<int> SetResult() {
			// 指定した時間が経過した後に結果を返すローカル関数
			Task<TResult> completeAsync<TResult>(TResult result, int milliseconds) {
				var source = new TaskCompletionSource<TResult>();

				// 指定時間後に結果を設定
				var fireAndForgetTask = Task.Delay(milliseconds).ContinueWith(_ => source.SetResult(result));

				return source.Task;
			}

			// 100ms後に結果を取得できる
			Assert.Equal(1, await completeAsync(1, 100));

			return 0;
		}

		[Fact]
		public async Task SetCanceled() {
			// 指定した時間が経過した後にキャンセルするローカル関数
			Task cancelAsync(int milliseconds) {
				var source = new TaskCompletionSource<int>();

				// 指定時間後にキャンセル
				var fireAndForgetTask = Task.Delay(milliseconds).ContinueWith(_ => source.SetCanceled());

				return source.Task;
			}

			// 100ms後にキャンセルされる
			await Assert.ThrowsAsync<TaskCanceledException>(async () => await cancelAsync(100));
		}

		[Fact]
		public async Task SetException() {
			// 指定した時間が経過した後に例外をスローするローカル関数
			Task throwAsync<TException>(TException exception, int milliseconds)
				where TException : Exception {
				var source = new TaskCompletionSource<int>();

				// 指定時間後に例外をスロー
				var fireAndForgetTask = Task.Delay(milliseconds).ContinueWith(_ => source.SetException(exception));

				return source.Task;
			}

			// 100ms後に例外スローされる
			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
				async () => await throwAsync(new ArgumentOutOfRangeException(), 100));
		}
	}
}
