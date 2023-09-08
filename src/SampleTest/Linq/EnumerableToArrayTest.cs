namespace SampleTest.Linq;

public class EnumerableToArrayTest {
	[Fact]
	public void ToArray_新しい配列インスタンスが生成される() {
		// Arrange
		var source = new[] { 1, 2, 3, 4, 5 };

		// Act
		var actual = source.ToArray();

		// Assert
		Assert.NotSame(source, actual);
		Assert.Equal(source, actual);
	}

	[Fact]
	public void ToArray_配列の要素はシャローコピーされる() {
		// Arrange
		var source = new[] { new { }, new { } };

		// Act
		var actual = source.ToArray();

		// Assert
		Assert.NotSame(source, actual);
		Assert.Equal(source, actual);
		// コピー前とコピー後の要素は同じインスタンス
		Assert.Same(source[0], actual[0]);
		Assert.Same(source[1], actual[1]);
	}
}
