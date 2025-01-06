namespace SampleTest.Linq;

public class EnumerableTakeTest {
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
		Assert.Equal([2, 3], actual);
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
}
