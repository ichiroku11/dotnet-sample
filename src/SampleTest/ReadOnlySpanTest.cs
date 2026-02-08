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
}
