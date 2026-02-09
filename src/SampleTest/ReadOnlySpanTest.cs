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
}
