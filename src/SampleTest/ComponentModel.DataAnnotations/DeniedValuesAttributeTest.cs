using System.ComponentModel.DataAnnotations;

namespace SampleTest.ComponentModel.DataAnnotations;

public class DeniedValuesAttributeTest {
	[Theory]
	// 指定した値（拒否する値）なので無効
	[InlineData("a", false)]
	[InlineData("b", false)]
	// 指定した値ではないので有効
	[InlineData(null, true)]
	[InlineData("A", true)]
	[InlineData("B", true)]
	public void IsValid_確認する(object? value, bool expected) {
		// Arrange
		var attribute = new DeniedValuesAttribute("a", "b");

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
