namespace SampleTest;

public class ReadOnlySpanTest {
	[Fact]
	public void Constructor_配列を指定し生成したインスタンスのプロパティを確認する() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var span = new ReadOnlySpan<int>(values);

		// Assert
		Assert.False(span.IsEmpty);
		Assert.Equal(3, span.Length);
		Assert.Equal([1, 2, 3], span);
	}

	[Fact]
	public void Constructor_配列と開始位置と長さを指定し生成したインスタンスのプロパティを確認する() {
		// Arrange
		var values = new[] { 1, 2, 3, 4, 5 };

		// Act
		var span = new ReadOnlySpan<int>(values, 1, 3);

		// Assert
		Assert.False(span.IsEmpty);
		Assert.Equal(3, span.Length);
		Assert.Equal([2, 3, 4], span);
	}

	[Fact]
	public void Constructor_要素単体を参照で指定し生成したインスタンスのプロパティを確認する() {
		// Arrange
		var value = 1;

		// Act
		var span = new ReadOnlySpan<int>(ref value);

		// Assert
		Assert.False(span.IsEmpty);
		Assert.Equal(1, span.Length);
		Assert.Equal([1], span);
	}

	[Fact]
	public void AsSpan_文字列からReadOnlySpanを取得する() {
		// Arrange

		// Act
		// 文字列の場合は、Span<char>ではなくReadOnlySpan<char>に変換されるメソッドのみが提供されている様子
		var span = "abc".AsSpan();

		// Assert
		Assert.Equal(new ReadOnlySpan<char>(['a', 'b', 'c']), span);
	}

	[Theory]
	[InlineData(1, true)]
	[InlineData(4, false)]
	public void Contains_指定した値が含まれているかどうか(int value, bool expected) {
		// Arrange
		ReadOnlySpan<int> span = [1, 2, 3];

		// Act
		var actual = span.Contains(value);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(1, 0, true)]
	[InlineData(0, 1, true)]
	[InlineData(1, 2, true)]
	[InlineData(0, 4, false)]
	public void ContainsAny_指定した値のどれかが含まれているかどうか(int value0, int value1, bool expected) {
		// Arrange
		ReadOnlySpan<int> span = [1, 2, 3];

		// Act
		var actual = span.ContainsAny(value0, value1);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void Implicit_配列を暗黙的にReadOnlySpanに型変換できる() {
		// Arrange

		// Act
		ReadOnlySpan<int> span = [1, 2, 3];

		// Assert
		Assert.Equal(new ReadOnlySpan<int>([1, 2, 3]), span);
	}

	[Fact]
	public void Implicit_Spanを暗黙的にReadOnlySpanに型変換できる() {
		// Arrange

		// Act
		ReadOnlySpan<int> span = new Span<int>([1, 2, 3]);

		// Assert
		Assert.Equal(new ReadOnlySpan<int>([1, 2, 3]), span);
	}

	[Theory]
	[InlineData(1, 0)]
	[InlineData(2, 1)]
	// 見つからない場合は-1
	[InlineData(0, -1)]
	[InlineData(4, -1)]
	public void IndexOf_指定した値の要素位置を取得する(int value, int expected) {
		// Arrange
		ReadOnlySpan<int> span = [1, 2, 3];

		// Act
		var actual = span.IndexOf(value);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0, 1, 0)]
	[InlineData(1, 0, 0)]
	// 3が見つかる位置は2、2が見つかる位置は1なので結果は1
	[InlineData(3, 2, 1)]
	// 見つからない場合は-1
	[InlineData(0, 4, -1)]
	public void IndexOfAny_指定した値がどれかが見つかるの最初の要素位置を取得する(int value0, int value1, int expected) {
		// Arrange
		ReadOnlySpan<int> span = [1, 2, 3];

		// Act
		var actual = span.IndexOfAny(value0, value1);

		// Assert
		Assert.Equal(expected, actual);
	}
}
