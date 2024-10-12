using System.Text;

namespace SampleTest.Text;

public class AsciiTest {
	[Theory]
	[InlineData("abcdefghijklmnopqrstuvwxyz", true)]
	public void IsValid(string value, bool expected) {
		// Arrange
		// Act
		var actual = Ascii.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
