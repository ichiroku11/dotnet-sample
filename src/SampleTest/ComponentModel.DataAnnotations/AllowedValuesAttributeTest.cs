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
	public void IsValid_文字列で確認する(string? value, bool expected) {
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
	public void IsValid_Int32で確認する(int value, bool expected) {
		// Arrange
		var attribute = new AllowedValuesAttribute(1);

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}

	// たぶん何を渡してもfalse
	[Theory]
	[InlineData(0)]
	[InlineData(null)]
	[InlineData(false)]
	[InlineData("")]
	public void IsValid_コンストラクターで引数を指定しない場合は必ずfalse(object? value) {
		// Arrange
		var attribute = new AllowedValuesAttribute();

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.False(actual);
	}
}
