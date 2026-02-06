namespace SampleTest;

public class SpanTest {
	[Fact]
	public void Constructor_配列を指定し生成したインスタンスのプロパティを確認する() {
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
	public void Constructor_配列と開始位置と長さを指定し生成したインスタンスのプロパティを確認する() {
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
	public void Constructor_要素単体を参照で指定し生成したインスタンスのプロパティを確認する() {
		// Arrange
		var value = 1;

		// Act
		var span = new Span<int>(ref value);

		// Assert
		Assert.False(span.IsEmpty);
		Assert.Equal(1, span.Length);
		Assert.Equal([1], span);
	}

	[Fact]
	public void AsSpan_配列からSpanを取得する() {
		// Arrange
		var values = new[] { 1, 2, 3 };

		// Act
		var span = values.AsSpan();

		// Assert
		Assert.Equal(new Span<int>(values), span);
	}

	[Fact]
	public void Clear_デフォルト値が設定される() {
		// Arrange
		var values = new[] { 1, 2, 3 };
		var span = new Span<int>(values);

		// Act
		span.Clear();

		// Assert
		// デフォルト値が設定される
		Assert.Equal([0, 0, 0], values);
		Assert.Equal(0, span[0]);
		Assert.Equal(0, span[1]);
		Assert.Equal(0, span[2]);

		// 「空になる」ことではない（最初勘違いした）
		Assert.False(span.IsEmpty);
		Assert.Equal(3, span.Length);
	}

	[Fact]
	public void Empty_空のSpanを取得できる() {
		// Arrange

		// Act
		var span = Span<int>.Empty;

		// Assert
		Assert.True(span.IsEmpty);
		Assert.Equal(0, span.Length);
	}

	[Fact]
	public void Empty_配列を受け取る空のSpanと等しい() {
		// Arrange

		// Act

		// Assert
		Assert.Equal(new Span<int>([]), Span<int>.Empty);
	}

	[Fact]
	public void Fill_指定した値で埋める() {
		// Arrange
		var values = new[] { 1, 2, 3 };
		var span = new Span<int>(values);

		// Act
		span.Fill(9);

		// Assert
		Assert.Equal([9, 9, 9], values);
	}

	[Fact]
	public void Slice_開始位置と長さを指定して部分的なSpanを取得する() {
		// Arrange
		var values = new[] { 1, 2, 3, 4, 5 };

		// Act
		var span = values.AsSpan().Slice(1, 3);

		// Assert
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
		Assert.Equal([1, 9, 3], values);
	}

	[Fact]
	public void Indexer_配列の要素が書き換わることを確認する_配列と開始位置と長さを受け取るコンストラクターで生成したインスタンス() {
		// Arrange
		var values = new int[] { 1, 2, 3, 4, 5 };
		var span = new Span<int>(values, 1, 3);

		// Act
		span[1] = 9;

		// Assert
		Assert.Equal([1, 2, 9, 4, 5], values);
	}
}
