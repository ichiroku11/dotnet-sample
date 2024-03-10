using Xunit;

namespace SampleLib.ComponentModel.DataAnnotations.Test;

public class AllowedEnumAttributeTest {
	private enum Sample {
		A = 1,
		B = 2
	}

	[Theory]
	// nullはtrue
	[InlineData(null, true)]
	// enumに変換できるのでtrue
	[InlineData("a", true)]
	[InlineData("A", true)]
	[InlineData("1", true)]
	// 数字なのでfalse
	[InlineData(1, false)]
	// enumに定義されていないのでfalse
	[InlineData("c", false)]
	public void IsValid_有効性を評価できる(object? value, bool expected) {
		// Arrange
		var attribute = new AllowedEnumAttribute<Sample>();

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
