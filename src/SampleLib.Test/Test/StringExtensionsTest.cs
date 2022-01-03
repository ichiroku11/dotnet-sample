using Xunit;

namespace SampleLib.Test;

public class StringExtensionsTest {
	[Theory]
	[InlineData("Border", "border")]
	[InlineData("BorderBlack", "border-black")]
	[InlineData("BorderGray50", "border-gray50")]
	[InlineData("borderBlack", "border-black")]
	public void ToKebabCase_ケバブケースに変換できる(string original, string expected) {
		// Arrange
		// Act
		var actual = original.ToKebabCase();

		// Assert
		Assert.Equal(expected, actual);
	}
}
