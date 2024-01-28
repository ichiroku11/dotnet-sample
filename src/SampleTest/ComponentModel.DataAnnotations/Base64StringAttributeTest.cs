using System.ComponentModel.DataAnnotations;

namespace SampleTest.ComponentModel.DataAnnotations;

public class Base64StringAttributeTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	[InlineData(null, true)]
	// 文字列以外はfalseになる
	[InlineData(false, false)]
	[InlineData(0, false)]
	// 空文字はtrueになる
	[InlineData("", true)]
	public void IsValid(object? value, bool expected) {
		// Arrange
		var attribute = new Base64StringAttribute();

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
