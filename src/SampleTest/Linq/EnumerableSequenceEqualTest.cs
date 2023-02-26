namespace SampleTest.Linq;

public class EnumerableSequenceEqualTest {
	[Theory]
	[InlineData(new[] { 0 }, null)]
	[InlineData(null, new[] { 0 })]
	public void SequenceEqual_引数のどちらかがnullだとArgumentNullExceptionが発生する(IEnumerable<int> first, IEnumerable<int> second) {
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentNullException>(() => {
			Enumerable.SequenceEqual(first, second);
		});
	}

	[Theory]
	[InlineData(new[] { 0 }, new[] { 0 }, true)]
	// お互い空の場合は等しい
	[InlineData(new int[] { }, new int[] { }, true)]
	// 順番が異なる場合は等しくない
	[InlineData(new[] { 0, 1 }, new[] { 1, 0 }, false)]
	public void SequenceEqual_いろいろな比較を試す(IEnumerable<int> first, IEnumerable<int> second, bool expected) {
		// Arrange
		// Act
		var actual = Enumerable.SequenceEqual(first, second);

		// Assert
		Assert.Equal(expected, actual);
	}
}
