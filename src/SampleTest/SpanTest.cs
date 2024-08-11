namespace SampleTest;

public class SpanTest {
	[Fact]
	public void Properties_プロパティを確認する_配列を受け取るコンストラクターで生成したインスタンス() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var span = new Span<int>(values);

		// Assert
		Assert.False(span.IsEmpty);
		Assert.Equal(3, span.Length);
		Assert.Equal([1, 2, 3], span);
	}

	[Fact]
	public void Properties_プロパティを確認する_配列と開始位置と長さを受け取るコンストラクターで生成したインスタンス() {
		// Arrange
		var values = new[] { 1, 2, 3, 4, 5 };

		// Act
		var span = new Span<int>(values, 1, 3);

		// Assert
		Assert.False(span.IsEmpty);
		Assert.Equal(3, span.Length);
		Assert.Equal([2, 3, 4], span);
	}

	[Fact]
	public void Indexer_配列の要素が書き換わることを確認する_配列を指定するコンストラクターで生成したインスタンス() {
		// Arrange
		var values = new int[] { 1, 2, 3 };
		var span = new Span<int>(values);

		// Act
		span[1] = 9;

		// Assert
		Assert.Equal(new[] { 1, 9, 3 }, values);
	}

	[Fact]
	public void Indexer_配列の要素が書き換わることを確認する_配列と開始位置と長さを受け取るコンストラクターで生成したインスタンス() {
		// Arrange
		var values = new int[] { 1, 2, 3, 4, 5 };
		var span = new Span<int>(values, 1, 3);

		// Act
		span[1] = 9;

		// Assert
		Assert.Equal(new[] { 1, 2, 9, 4, 5 }, values);
	}
}
