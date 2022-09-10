namespace SampleTest.Linq;

public class EnumerableOrderByTest {
	// boolを並び替えると
	// false => trueという並び順になる
	[Theory]
	[InlineData(new[] { true, false }, new[] { false, true })]
	[InlineData(new[] { false, true }, new[] { false, true })]
	public void OrderBy_boolの並び順を確認する(IEnumerable<bool> source, IEnumerable<bool> expected) {
		// Arrange
		// Act
		var actual = source.OrderBy(value => value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
