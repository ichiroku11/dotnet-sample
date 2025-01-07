namespace SampleTest.Linq;

public class EnumerableIndexTest {
	[Fact]
	public void Index_インデックスと要素を含んだタプルを返す() {
		// Arrange
		var values = new char[] { 'a', 'b', 'c', 'd' };

		// Act
		var actual = values.Index();

		// Assert
		Assert.Equal([(0, 'a'), (1, 'b'), (2, 'c'), (3, 'd')], actual);
	}
}
