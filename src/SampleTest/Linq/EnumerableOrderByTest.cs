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

	// nullとnot nullを並び替えると
	// null => not nullという並び順になる
	[Fact]
	public void OrderBy_nullとintの並び順を確認する() {
		// Arrange
		var source = new int?[] { int.MinValue, null };

		// Act
		var actual = source.OrderBy(value => value);

		// Assert
		var expected = new int?[] { null, int.MinValue };
		Assert.Equal(expected, actual);
	}
}
