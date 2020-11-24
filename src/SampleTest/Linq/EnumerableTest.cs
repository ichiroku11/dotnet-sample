using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SampleTest.Linq {
	public class EnumerableTest {
		[Fact]
		public void Prepend_シーケンスの最初に要素を追加する() {
			// Arrange
			var source = new[] { 2, 3, 4 };

			// Act
			var actual = source.Prepend(1);

			// Assert
			Assert.Equal(new[] { 1, 2, 3, 4 }, actual);
		}

		[Fact]
		public void Append_シーケンスの最後に要素を追加する() {
			// Arrange
			var source = new[] { 2, 3, 4 };

			// Act
			var actual = source.Append(5);

			// Assert
			Assert.Equal(new[] { 2, 3, 4, 5 }, actual);
		}

		[Fact]
		public void Min_非nullの空のシーケンスで呼び出すとInvalidOperationException() {
			// 非nullの場合は例外
			Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Min());

			// null許容型の場合はnull
			Assert.Null(Enumerable.Empty<int?>().Min());
		}

		[Fact]
		public void Max_非nullの空のシーケンスで呼び出すとInvalidOperationException() {
			// 非nullの場合は例外
			Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Max());

			// null許容型の場合はnull
			Assert.Null(Enumerable.Empty<int?>().Max());
		}

		[Fact]
		public void Average_非nullの空のシーケンスで呼び出すとInvalidOperationException() {
			// 非nullの場合は例外
			Assert.Throws<InvalidOperationException>(() => Enumerable.Empty<int>().Average());

			// null許容型の場合はnull
			Assert.Null(Enumerable.Empty<int?>().Average());
		}

		[Fact]
		public void Take_引数にマイナスの値を指定すると空のシーケンスが返ってくる() {
			// Arrange
			var source = new[] { 1, 2, 3 };

			// Act
			var actual = source.Take(-1);

			// Assert
			Assert.Empty(actual);
		}

		[Fact]
		public void Skip_引数にマイナスの値を指定すると同じシーケンスが返ってくる() {
			// Arrange
			var source = new[] { 1, 2, 3 };

			// Act
			var actual = source.Skip(-1);

			// Assert
			Assert.NotSame(source, actual);
			Assert.Equal(source, actual);
		}

		[Fact]
		public void SequenceEqual_2つのコレクションが等しいか比較できる() {
			// Arrange
			var first = new[] { 1, 2, 3 };
			var second = new[] { 1, 2, 3 };

			// Act
			// Assert
			Assert.True(first.SequenceEqual(second));
		}

		private interface ISample {
		}

		private class Sample : ISample {
		}

		[Fact]
		public void ToList_クラスのListをインターフェイスのIListに変換する() {
			// Arrange
			var items = new List<Sample> { new Sample() };

			// Act
			// Assert
			Assert.True(items.ToList<ISample>() is List<ISample>);
		}

		[Fact]
		public void Single_シーケンスが空だとInvalidOperationExceptionがスロー() {
			// Arrange
			// Act
			// Assert
			Assert.Throws<InvalidOperationException>(() => {
				Enumerable.Empty<int>().Single();
			});
		}

		[Fact]
		public void Single_シーケンスの要素が2つ以上だとInvalidOperationExceptionがスロー() {
			// Arrange
			// Act
			// Assert
			Assert.Throws<InvalidOperationException>(() => {
				Enumerable.Repeat(0, 2).Single();
			});
		}

		[Fact]
		public void SingleOrDefault_シーケンスが空の場合は例外がスローされずデフォルト値を取得できる() {
			// Arrange
			// Act
			var value = Enumerable.Empty<int>().SingleOrDefault();
			// Assert
			Assert.Equal(default, value);
		}

		[Fact]
		public void SingleOrDefault_シーケンスの要素が2つ以上だとInvalidOperationExceptionがスロー() {
			// Arrange
			// Act
			// Assert
			Assert.Throws<InvalidOperationException>(() => {
				Enumerable.Repeat(0, 2).SingleOrDefault();
			});
		}

		private static IEnumerable<int> Range(int start, int count, Action<int> action) {
			for (var index = start; index < count; index++) {
				action(index);
				yield return index;
			}
		}

		[Fact]
		public void Any_シーケンスの列挙を途中で中止する() {
			// Arrange
			var values = new List<int>();

			// Act
			var any = Range(0, 5, value => values.Add(value)).Any();

			// Assert
			Assert.True(any);
			Assert.Equal(new List<int> { 0 }, values);
		}

		[Fact]
		public void Any_条件を満たす要素が見つかればシーケンスの列挙を途中で中止する() {
			// Arrange
			var values = new List<int>();

			// Act
			var any = Range(0, 5, value => values.Add(value)).Any(value => value >= 2);

			// Assert
			Assert.True(any);
			Assert.Equal(new List<int> { 0, 1, 2 }, values);
		}

		[Fact]
		public void Any_条件を満たす要素が見つからないのでシーケンスを最後まで列挙する() {
			// Arrange
			var values = new List<int>();

			// Act
			var any = Range(0, 5, value => values.Add(value)).Any(value => value == -1);

			// Assert
			Assert.False(any);
			Assert.Equal(new List<int> { 0, 1, 2, 3, 4 }, values);
		}
	}
}
