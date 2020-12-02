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

		public static IEnumerable<object[]> GetTestDataForZip() {
			yield return new object[] {
				// first
				new[] { 1, 2, 3 },
				// second
				new[] { 6, 5, 4 },
				// expected
				new [] { (1, 6), (2, 5), (3, 4) },
			};

			// 2つのシーケンスの要素数が異なる場合は、少ない方の要素数で列挙される
			// first < second
			yield return new object[] {
				new[] { 1, 2, 3 },
				new[] { 6, 5, 4, -1 },
				new [] { (1, 6), (2, 5), (3, 4) },
			};
			// first > second
			yield return new object[] {
				new[] { 1, 2, 3, -1 },
				new[] { 6, 5, 4 },
				new [] { (1, 6), (2, 5), (3, 4) },
			};
		}

		[Theory]
		[MemberData(nameof(GetTestDataForZip))]
		public void Zip_2つのシーケンスからタプルを生成する(IEnumerable<int> first, IEnumerable<int> second, IEnumerable<(int, int)> expected) {
			// Arrange
			// Act
			var actual = first.Zip(second);

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
