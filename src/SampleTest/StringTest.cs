namespace SampleTest;

public class StringTest {
	[Theory]
	// 空文字とnullは等しくない
	[InlineData("", null, false)]
	[InlineData(null, "", false)]
	// nullとnullは等しい
	[InlineData(null, null, true)]
	public void Equals_引数がnullでも比較できる(string? left, string? right, bool expected) {
		// Arrange
		// Act
		var actual = string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

		// Assert
		Assert.Equal(expected, actual);
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
