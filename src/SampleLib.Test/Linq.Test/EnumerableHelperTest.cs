using Xunit;

namespace SampleLib.Linq.Test;

public class EnumerableHelperTest {
	[Theory]
	[InlineData(null, null, true)]
	[InlineData(new[] { 0 }, new[] { 0 }, true)]
	[InlineData(null, new[] { 0 }, false)]
	[InlineData(new[] { 0 }, null, false)]
	public void BothNullOrSequenceEqual_いろいろな比較を試す(IEnumerable<int> first, IEnumerable<int> second, bool expected) {
		// Arrange
		// Act
		var actual = EnumerableHelper.BothNullOrSequenceEqual(first, second);

		// Assert
		Assert.Equal(expected, actual);
	}
}
