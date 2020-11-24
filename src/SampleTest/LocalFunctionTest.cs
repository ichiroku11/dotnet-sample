using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest {
	// ローカル関数のテスト
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/programming-guide/classes-and-structs/local-functions
	public partial class LocalFunctionTest {
		#region イテレータでローカル関数を使うサンプル
		// 指定した範囲のシーケンスを作成する（ローカル関数を使わない）
		// メソッドを呼び出した直後には例外がスローされず
		// 列挙されると例外がスローされる
		private IEnumerable<int> OldstyleRange(int start, int count) {
			if (count < 0) {
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			foreach (var item in Enumerable.Range(start, count)) {
				yield return item;
			}
		}

		[Fact]
		public void OldstyleRange_ローカル関数を使わない場合は列挙されると例外がスローされる() {
			// メソッドを呼び出した直後に例外がスローされない
			var items = OldstyleRange(0, -1);

			// 列挙されると例外がスローされる
			Assert.Throws<ArgumentOutOfRangeException>(() => {
				foreach (var item in items) {
				}
			});
		}

		// 指定した範囲のシーケンスを作成する（ローカル関数を使う）
		// メソッドを呼び出した直後に例外がスローされる
		private IEnumerable<int> AwesomeRange(int start, int count) {
			if (count < 0) {
				throw new ArgumentOutOfRangeException(nameof(count));
			}

			// ローカル関数の定義
			IEnumerable<int> range() {
				foreach (var item in Enumerable.Range(start, count)) {
					yield return item;
				}
			}

			// ローカル関数の呼び出し
			return range();
		}

		[Fact]
		public void AwesomeRange_ローカル関数を使うとメソッドを呼び出した直後に例外がスローされる() {
			// メソッドを呼び出した直後に例外がスローされる
			Assert.Throws<ArgumentOutOfRangeException>(() => {
				AwesomeRange(0, -1);
			});
		}
		#endregion

		#region 非同期メソッドでローカル関数を使うサンプル
		// 何かしらの読み込み処理（ローカル関数を使わない）
		private async Task<int> OldstyleLoadAsync(string key) {
			if (string.IsNullOrWhiteSpace(key)) {
				throw new ArgumentException(nameof(key));
			}

			// 重たい処理
			await Task.Delay(1000);

			return 1;
		}

		[Fact]
		public void OldstyleLoadAsync_ローカル関数を使わない場合は列挙されると例外がスローされる() {
			// メソッドを呼び出した直後に例外がスローされない
			var task = OldstyleLoadAsync(null);

			// タスクが完了すると例外がスロー
			var exception = Assert.Throws<AggregateException>(() => {
				task.Wait();
			});
			Assert.Single(exception.InnerExceptions);
			Assert.IsType<ArgumentException>(exception.InnerException);
		}

		// 何かしらの読み込み処理（ローカル関数を使う）
		private Task<int> AwesomeLoadAsync(string key) {
			if (string.IsNullOrWhiteSpace(key)) {
				throw new ArgumentException(nameof(key));
			}

			// ローカル関数の定義
			async Task<int> load() {
				// 重たい処理
				await Task.Delay(1000);

				return 1;
			}

			// ローカル関数の呼び出し
			return load();
		}

		[Fact]
		public void AwesomeLoadAsync_ローカル関数を使うとメソッドを呼び出した直後に例外がスローされる() {
			var task = default(Task);

			// メソッドを呼び出した直後に例外がスローされる
			// しかも例外はAggregateExceptionではない
			var exception = Assert.Throws<ArgumentException>(() => {
				task = AwesomeLoadAsync(null);
			});

			Assert.Null(task);
		}
		#endregion
	}
}
