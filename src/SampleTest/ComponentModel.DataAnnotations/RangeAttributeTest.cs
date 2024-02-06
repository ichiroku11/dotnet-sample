using System.ComponentModel.DataAnnotations;

namespace SampleTest.ComponentModel.DataAnnotations;

public class RangeAttributeTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

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

	public static TheoryData<RangeAttribute> GetTheoryData_IsValid()
		=> new() {
			// 最小値が最大値より大きい
			new RangeAttribute(2, 1),
			// 範囲内の数値が存在しない
			new RangeAttribute(1, 1) { MinimumIsExclusive = true },
			new RangeAttribute(1, 1) { MaximumIsExclusive = true },
		};

	[Theory]
	[MemberData(nameof(GetTheoryData_IsValid))]
	public void IsValid_指定した最小値と最大値が不正な場合は例外が発生することを確認する(RangeAttribute attribute) {
		// Arrange

		// Act
		var exception = Record.Exception(() => attribute.IsValid(null));

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
		_output.WriteLine(exception.Message);
	}
}
