using System.ComponentModel.DataAnnotations;

namespace SampleTest.ComponentModel.DataAnnotations;

public class RangeAttributeTest {
	[Theory]
	[InlineData(0, true)]
	[InlineData(10, true)]
	public void IsValid_境界値が有効であることを確認する(int value, bool expected) {
		// Arrange
		var attribute = new RangeAttribute(0, 10);

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0, false)]
	[InlineData(10, false)]
	public void IsValid_境界値が無効であることを確認する(int value, bool expected) {
		// Arrange
		var attribute = new RangeAttribute(0, 10) {
			// 境界値を含めない
			MinimumIsExclusive = true,
			MaximumIsExclusive = true
		};

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
