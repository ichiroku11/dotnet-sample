using System.ComponentModel.DataAnnotations;

namespace SampleTest.ComponentModel.DataAnnotations;

public class LengthAttributeTest {
	[Theory]
#pragma warning disable CA1861 // Avoid constant arrays as arguments
	[InlineData(new[] { 0 }, false)]
	[InlineData(new[] { 0, 1 }, true)]
	[InlineData(new[] { 0, 1, 2 }, true)]
	[InlineData(new[] { 0, 1, 2, 3 }, false)]
#pragma warning restore CA1861 // Avoid constant arrays as arguments
	public void IsValid_配列で長さの有効性を確認する(int[] values, bool expected) {
		// Arrange
		var attribute = new LengthAttribute(2, 3);

		// Act
		var actual = attribute.IsValid(values);

		// Assert
		Assert.Equal(expected, actual);
	}
}
