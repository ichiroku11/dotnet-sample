using System.ComponentModel.DataAnnotations;

namespace SampleTest.ComponentModel.DataAnnotations;

public class AllowedValuesAttributeTest {
	[Theory]
	// 指定した値（許可する値）なので有効
	[InlineData("a", true)]
	[InlineData("b", true)]
	// 指定した値ではないので無効
	[InlineData(null, false)]
	[InlineData("A", false)]
	[InlineData("B", false)]
	public void IsValid_確認する(object? value, bool expected) {
		// Arrange
		var attribute = new AllowedValuesAttribute("a", "b");

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(1, true)]
	[InlineData(2, false)]
	public void IsValid_Int32で確認する(object? value, bool expected) {
		// Arrange
		var attribute = new AllowedValuesAttribute(1);

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
