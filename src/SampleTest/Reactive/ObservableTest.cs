using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Reactive {
	// 参考
	// http://reactivex.io/documentation/operators.html
	// https://www.atmarkit.co.jp/fdotnet/introrx/introrx_02/introrx_02_01.html
	// https://blog.okazuki.jp/entry/20111104/1320409976
	public class ObservableTest {
		[Fact]
		public void Subscribe_リターンされたIDisposableのDisposeメソッドはUnsubscribeという意味() {
			// Arrange
			var disposed = false;
			var observable = Observable.Create<int>(observer => {
				// Disposeが呼ばれたときの処理
				return () => {
					// ここでリソースの解放などをすると良いらしい
					disposed = true;
				};
			});
			var completed = false;

			// Act
			var disposable = observable.Subscribe(
				onNext: _ => AssertHelper.Fail(),
				onError: _ => AssertHelper.Fail(),
				onCompleted: () => {
					completed = true;
				});

			// Assert
			Assert.False(disposed);
			Assert.False(completed);

			// IDisposable.DisposeはUnsubscribeの意味
			disposable.Dispose();
			Assert.True(disposed);
			// onCompletedは呼ばれない
			Assert.False(completed);
		}
	}
}
