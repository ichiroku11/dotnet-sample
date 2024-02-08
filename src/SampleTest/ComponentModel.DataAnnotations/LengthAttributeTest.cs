using System.ComponentModel.DataAnnotations;

namespace SampleTest.ComponentModel.DataAnnotations;

public class LengthAttributeTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	[InlineData(null, true)]
#pragma warning disable CA1861 // Avoid constant arrays as arguments
	[InlineData(new[] { 0 }, false)]
	[InlineData(new[] { 0, 1 }, true)]
	[InlineData(new[] { 0, 1, 2 }, true)]
	[InlineData(new[] { 0, 1, 2, 3 }, false)]
#pragma warning restore CA1861 // Avoid constant arrays as arguments
	public void IsValid_配列で長さの有効性を確認する(int[]? values, bool expected) {
		// Arrange
		var attribute = new LengthAttribute(2, 3);

		// Act
		var actual = attribute.IsValid(values);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(null, true)]
	[InlineData("a", false)]
	[InlineData("ab", true)]
	[InlineData("abc", true)]
	[InlineData("abcd", false)]
	public void IsValid_文字列で長さの有効性を確認する(string? text, bool expected) {
		// Arrange
		var attribute = new LengthAttribute(2, 3);

		// Act
		var actual = attribute.IsValid(text);

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<LengthAttribute> GetTheoryData_IsValid()
		=> new() {
			// インスタンスを作るときは例外が発生せず
			// IsValidを呼び出すときに例外が発生する
			// 最小値がマイナス
			new LengthAttribute(-1, 1),
			// 最小値が最大値より大きい
			new LengthAttribute(2, 1),
		};

	[Theory]
	[MemberData(nameof(GetTheoryData_IsValid))]
	public void IsValid_指定した最小値と最大値が不正な場合は例外が発生することを確認する(LengthAttribute attribute) {
		// Arrange

		// Act
		var exception = Record.Exception(() => attribute.IsValid(null));

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
		_output.WriteLine(exception.Message);
	}
}
