using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Threading {
	public class SemaphoreSlimTest {
		private readonly ITestOutputHelper _output;

		public SemaphoreSlimTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void 使ってみる() {
			// Arrange
			// Act
			// Assert
			using (var semaphore = new SemaphoreSlim(0)) {
				Assert.Equal(0, semaphore.CurrentCount);
			}
		}

		[Fact]
		public void Release_セマフォに残っている数が増える() {
			// Arrange
			// Act
			// Assert
			using (var semaphore = new SemaphoreSlim(0)) {
				Assert.Equal(0, semaphore.CurrentCount);

				// リリースするごとにセマフォに残っている数は増える
				semaphore.Release();
				Assert.Equal(1, semaphore.CurrentCount);

				semaphore.Release();
				Assert.Equal(2, semaphore.CurrentCount);
			}
		}

		[Fact]
		public void Release_最大値までセマフォに残っている数が増える() {
			// Arrange
			// Act
			// Assert
			// 初期値と最大値を指定
			using (var semaphore = new SemaphoreSlim(0, 1)) {
				Assert.Equal(0, semaphore.CurrentCount);

				// リリースするとセマフォに残っている数が増えるが
				semaphore.Release();
				Assert.Equal(1, semaphore.CurrentCount);

				// 最大値を超えてリリースすると例外がスロー
				Assert.Throws<SemaphoreFullException>(() => {
					semaphore.Release();
				});
			}
		}
		
		[Fact]
		public async Task CurrentCount_0以下の値にはならない様子() {
			// Arrange
			// Act
			// Assert
			// 初期値と最大値を指定
			using (var semaphore = new SemaphoreSlim(1)) {
				Assert.Equal(1, semaphore.CurrentCount);

				// WaitするとCurrentCountが1つ減る
				await semaphore.WaitAsync();
				Assert.Equal(0, semaphore.CurrentCount);

				// さらにWaitすると完了しないタスクが返り、
				// CurrentCountは0のまま（最小が0かも）
				var task = semaphore.WaitAsync();
				Assert.False(task.IsCompleted);
				Assert.Equal(0, semaphore.CurrentCount);

				// ReleaseしてもCurrentCountは0のまま
				semaphore.Release();
				Assert.Equal(0, semaphore.CurrentCount);
				await task;

				// さらにReleaseするとCurrentCountが1増える
				semaphore.Release();
				Assert.Equal(1, semaphore.CurrentCount);
			}
		}
		
		// しっくりこないかも・・・
		[Fact]
		public void Release_WaitAsyncしたタスクにReleaseで通知する() {
			// Arrange
			// Act
			// Assert
			using (var source = new CancellationTokenSource())
			using (var semaphore = new SemaphoreSlim(0)) {
				var task = Task.Run(async () => {
					_output.WriteLine($"WaitAsync Before");

					source.Cancel();
					await semaphore.WaitAsync();

					_output.WriteLine($"WaitAsync After");
				});

				source.Token.Register(() => {
					// タスクは完了していない
					Assert.False(task.IsCompleted);

					_output.WriteLine($"Release Before");

					// Releaseすると待機してたタスクが動き出す
					semaphore.Release();

					_output.WriteLine($"Release After");
				});

				Task.WaitAll(task);
			}
		}

		[Fact]
		public async Task WaitAsync_戻り値はセマフォに入れると完了済みのTaskになり入れないと未完了タスクになる() {
			// Arrange
			// Act
			// Assert
			using (var semaphore = new SemaphoreSlim(1)) {
				// タスク1は完了している
				var task1 = semaphore.WaitAsync();
				Assert.True(task1.IsCompleted);
				await task1;

				// タスク2は完了していない
				var task2 = semaphore.WaitAsync();
				Assert.False(task2.IsCompleted);

				// リリースするとタスク2が完了する
				semaphore.Release();
				await task2;
				Assert.True(task2.IsCompleted);
			}
		}

		// todo: これダメだ
		/*
		[Fact]
		public async Task 使ってみる2() {
			// Arrange
			// Act
			// Assert
			using (var semaphore = new SemaphoreSlim(1)) {
				var step = 0;

				var task = Task.Run(async () => {
					await semaphore.WaitAsync();

					Interlocked.Increment(ref step);
				});

				Assert.Equal(0, step);

				semaphore.Release();

				Assert.Equal(1, step);

				await task;
			}
		}
		*/

		// todo: 上手く書けない
		/*
		[Fact]
		public void SemaphoreSlimを使ってみる2() {
			// Arrange
			// Act
			// Assert
			var semaphore = new SemaphoreSlim(0);

			var task1 = Task.Run(() => {
				_output.WriteLine("Before wait");
				semaphore.Wait();
				_output.WriteLine("After wait");
				return 1;
			});
			var task2 = Task.Run(() => {
				Thread.Sleep(500);
				_output.WriteLine("Before release");
				semaphore.Release();
				_output.WriteLine("After release");
			});

			Assert.Equal(1, task1.Result);
		}
		*/
	}
}
