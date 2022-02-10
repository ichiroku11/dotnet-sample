using Xunit;

namespace SampleTest.Linq;

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
	public void Take_Range型で指定してシーケンスを取得する() {
		// Arrange
		var source = new[] { 0, 1, 2, 3, 4, 5 };

		// Act
		// 2番目から4番目まで（4番目は含まない）を取得
		var actual = source.Take(2..4);

		// Assert
		Assert.Equal(new[] { 2, 3 }, actual);
	}

	[Fact]
	public void Take_Range型で指定したが要素が見つからない場合は空が返ってくる() {
		// Arrange
		var source = new[] { 0, 1, 2, 3 };

		// Act
		// 2番目から4番目まで（4番目は含まない）を取得
		var actual = source.Take(5..);

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
	public void TryGetNonEnumeratedCount_配列の場合はカウントを取得できる() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var actualReturn = values.TryGetNonEnumeratedCount(out var actualCount);

		// Assert
		Assert.True(actualReturn);
		Assert.Equal(3, actualCount);
	}

	[Fact]
	public void TryGetNonEnumeratedCount_Enumerableの場合はカウントを取得できる() {
		// Arrange
		var values = Enumerable.Range(1, 3);

		// Act
		var actualReturn = values.TryGetNonEnumeratedCount(out var actualCount);

		// Assert
		Assert.True(actualReturn);
		Assert.Equal(3, actualCount);
	}

	[Fact]
	public void TryGetNonEnumeratedCount_yieldreturnの場合はカウントを取得できない() {
		// Arrange
		static IEnumerable<int> values(int count) {
			foreach (var value in Enumerable.Range(0, count)) {
				yield return value;
			}
		};

		// Act
		var actualReturn = values(3).TryGetNonEnumeratedCount(out var _);

		// Assert
		Assert.False(actualReturn);
	}
}
