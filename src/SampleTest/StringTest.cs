using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest;

public class StringTest {
	[Fact]
	public void Equals_引数がnullでも比較できる() {
		// Arrange
		// Act
		// Assert
		Assert.False(string.Equals("", null, StringComparison.OrdinalIgnoreCase));
		Assert.False(string.Equals(null, "", StringComparison.OrdinalIgnoreCase));

		Assert.True(string.Equals(null, null, StringComparison.OrdinalIgnoreCase));
	}

	[Theory]
	[InlineData("1", 8, '0', "00000001")]
	[InlineData("11111111", 8, '0', "11111111")]
	public void PadLeft_左に文字を埋め込んだ文字列を取得できる(string src, int totalWidth, char paddingChar, string expected) {
		// Arrange
		// Act
		var actual = src.PadLeft(totalWidth, paddingChar);

		// Assert
		Assert.Equal(expected, actual);
	}
}
