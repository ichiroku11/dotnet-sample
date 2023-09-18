namespace SampleTest.Linq;

public class EnumerableAllTest {
	// https://qiita.com/whopper1962/items/53b8f07c4571b8cfb2ff
	[Fact]
	public void All_空のコレクションの場合は条件を満たさない要素が存在しないのでtrue() {
		// Arrange
		// Act
		var actual = Enumerable.Empty<int>().All(_ => false);

		// Assert
		Assert.True(actual);
	}
}
