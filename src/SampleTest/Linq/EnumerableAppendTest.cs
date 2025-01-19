namespace SampleTest.Linq;

public class EnumerableAppendTest {
	[Fact]
	public void Append_シーケンスの最後に要素を追加する() {
		// Arrange
		var source = new[] { 2, 3, 4 };

		// Act
		var actual = source.Append(5);

		// Assert
		Assert.Equal([2, 3, 4, 5], actual);
	}
}
