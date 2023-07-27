using Microsoft.Extensions.Configuration;

namespace SampleTest.Extensions.Configuration;

public class ConfigurationPathTest {
	[Fact]
	public void KeyDelimiter_値を確認する() {
		// Arrange
		// Act
		// Assert
		Assert.Equal(":", ConfigurationPath.KeyDelimiter);
	}

	[Theory]
	[InlineData("x:y:z", "x", "y", "z")]
	public void Combine_パスのセグメントを結合する(string expected, params string[] segments) {
		// Arrange
		// Act
		var actual = ConfigurationPath.Combine(segments);

		// Assert
		Assert.Equal(expected, actual);
	}
}
