namespace SampleTest.Linq;

public class EnumerablePrependTest {
	[Fact]
	public void Prepend_シーケンスの最初に要素を追加する() {
		// Arrange
		var source = new[] { 2, 3, 4 };

		// Act
		var actual = source.Prepend(1);

		// Assert
		Assert.Equal([1, 2, 3, 4], actual);
	}
}
