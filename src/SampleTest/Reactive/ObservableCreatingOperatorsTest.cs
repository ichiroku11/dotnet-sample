using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Reactive {
	// 参考
	// https://www.atmarkit.co.jp/fdotnet/introrx/introrx_02/introrx_02_01.html
	// https://blog.okazuki.jp/entry/20111104/1320409976
	public class ObservableCreatingOperatorsTest {
		private readonly ITestOutputHelper _output;

		public ObservableCreatingOperatorsTest(ITestOutputHelper output) {
			_output = output;
		}

		// todo:
		// Defer
		// Interval
		// Timer
		// Using

		[Fact]
		public void Create_任意のObservableを生成する() {
			// Arrange
			var disposed = false;
			var observable = Observable.Create<int>(observer => {
				observer.OnNext(1);
				observer.OnNext(2);
				observer.OnNext(3);

				// OnCompletedでDisposeが呼ばれる
				observer.OnCompleted();

				// Disposeが呼ばれたときの処理
				return () => {
					disposed = true;
				};
			});
			var values = new List<int>();
			var completed = false;

			// Act
			observable.Subscribe(
				onNext: value => values.Add(value),
				onError: _ => AssertHelper.Fail(),
				onCompleted: () => completed = true);

			// Assert
			Assert.True(disposed);
			Assert.True(completed);
			Assert.Equal(new List<int> { 1, 2, 3 }, values);
		}

		[Fact]
		public void Empty_onCompletedだけが呼ばれる() {
			// Arrange
			var completed = false;

			// Act
			Observable.Empty<int>().Subscribe(
				onNext: _ => AssertHelper.Fail(),
				onError: _ => AssertHelper.Fail(),
				// onCompletedだけが呼ばれる
				onCompleted: () => {
					Assert.False(completed);
					completed = true;
				});

			// Assert
			Assert.True(completed);
		}

		[Fact]
		public void FromAsync_TaskをObservableに変換するとonNextとonCompletedが呼ばれる() {
			// Arrange
			var expected = 11;
			var actual = 0;
			var completed = false;

			// Act
			Observable.FromAsync(() => Task.FromResult(expected)).Subscribe(
				onNext: value => {
					actual = value;
				},
				onCompleted: () => {
					Assert.False(completed);
					completed = true;
				});

			// Assert
			Assert.True(completed);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void Generate_for文のようなイメージでシーケンスを生成できる() {
			// Arrange
			var completed = false;
			var values = new List<int>();

			// Act
			Observable
				.Generate(
					initialState: 1,
					condition: value => value <= 3,
					iterate: value => value + 1,
					resultSelector: value => value * 2)
				.Subscribe(
					onNext: value => values.Add(value),
					onError: _ => AssertHelper.Fail(),
					onCompleted: () => completed = true);

			// Assert
			Assert.True(completed);
			Assert.Equal(new List<int> { 2, 4, 6 }, values);
		}

		[Fact]
		public void Never_何も呼ばれない() {
			// Arrange
			// Act
			// Assert
			Observable.Never<int>().Subscribe(
				onNext: _ => AssertHelper.Fail(),
				onError: _ => AssertHelper.Fail(),
				onCompleted: () => AssertHelper.Fail());
		}

		[Fact]
		public void Range_onNextとonCompletedが呼ばれる() {
			// Arrange
			var values = new List<int>();
			var completed = false;

			// Act
			Observable.Range(0, 3).Subscribe(
				onNext: value => {
					// 0, 1, 2と呼ばれる
					_output.WriteLine($"onNext: {value}");

					// onCompletedは呼ばれていない
					Assert.False(completed);

					values.Add(value);
				},
				onError: _ => {
					// エラーが発生しないので呼ばれない
					AssertHelper.Fail();
				},
				onCompleted: () => {
					_output.WriteLine($"onCompleted");
					// onCompletedは初めて呼ばれる
					Assert.False(completed);

					completed = true;
				});

			// Assert
			Assert.Equal(new List<int>() { 0, 1, 2 }, values);
			Assert.True(completed);
		}

		[Fact]
		public void Repeat_onNextとonCompletedが呼ばれる() {
			// Arrange
			var values = new List<int>();
			var completed = false;

			// Act
			Observable.Repeat(1, 3).Subscribe(
				onNext: value => {
					Assert.False(completed);

					_output.WriteLine($"onNext: {value}");
					values.Add(value);
				},
				onError: _ => {
					AssertHelper.Fail();
				},
				onCompleted: () => {
					Assert.False(completed);

					_output.WriteLine($"onCompleted");
					completed = true;
				});

			// Assert
			Assert.Equal(new List<int>() { 1, 1, 1 }, values);
			Assert.True(completed);
		}

		[Fact]
		public void Return_onNextとonCompletedが呼ばれる() {
			// Arrange
			var values = new List<int>();
			var completed = false;

			// Act
			Observable.Return(1).Subscribe(
				onNext: value => {
					Assert.False(completed);

					_output.WriteLine($"onNext: {value}");
					values.Add(value);
				},
				onError: _ => {
					AssertHelper.Fail();
				},
				onCompleted: () => {
					Assert.False(completed);

					_output.WriteLine($"onCompleted");
					completed = true;
				});

			// Assert
			Assert.Equal(new List<int>() { 1 }, values);
			Assert.True(completed);
		}

		[Fact]
		public void Throw_onErrorだけが呼ばれる() {
			// Arrange
			var expected = new Exception();
			var actual = default(Exception);

			// Act
			Observable.Throw<int>(expected).Subscribe(
				onNext: _ => AssertHelper.Fail(),
				// onErrorだけが呼ばれる
				onError: exception => actual = exception,
				onCompleted: () => AssertHelper.Fail());

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
