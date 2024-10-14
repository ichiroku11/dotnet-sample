using System.Text;

namespace SampleTest.Text;

public class AsciiTest {
	[Theory]
	[InlineData("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~", true)]
	[InlineData("0123456789", true)]
	[InlineData("abcdefghijklmnopqrstuvwxyz", true)]
	[InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", true)]
	// 全角の"０"を含む
	[InlineData("０", false)]
	public void IsValid_ASCII文字列のみを含むか判定できる(string value, bool expected) {
		// Arrange
		// Act
		var actual = Ascii.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
