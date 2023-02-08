namespace SampleTest;

public class ConvertTest {
	// 参考
	// https://www.rfc-editor.org/rfc/rfc4648
	[Fact]
	public void ToBase64String_変換を確認する() {
		// Arrange
		var bytes = new byte[] {
			0b_1111_1000
		};

		// Act
		var actual = Convert.ToBase64String(bytes);

		// Assert
		// https://www.rfc-editor.org/rfc/rfc4648#section-4
		// 6ビットずつに分割（足りない場合は0を追加）
		// 111010(62) => "+"
		// 000000(0) => "A"
		// 4文字に足りない => "=="を追加
		Assert.Equal("+A==", actual);
	}

	[Theory]
	// 大文字・小文字どちらでもバイト配列に変換できる
	[InlineData("AB", new byte[] { 0b_1010_1011 })]
	[InlineData("ab", new byte[] { 0b_1010_1011 })]
	public void FromHexString_16進数文字列をバイト配列に変換できる(string hexString, byte[] expected) {
		// Arrange
		// Act
		var actual = Convert.FromHexString(hexString);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void ToHexString_バイト配列を大文字の16進数文字列に変換できる() {
		// Arrange
		// Act
		var actual = Convert.ToHexString(new byte[] { 0b_1010_1011 });

		// Assert
		Assert.Equal("AB", actual);
	}


	[Theory]
	[InlineData("1", 1)]
	[InlineData("0001", 1)] // 先頭に0があっても大丈夫
	[InlineData("11111111", 255)]
	public void ToInt32_文字列を2進数としてInt32に変換できる(string text, int expected) {
		// Arrange
		// Act
		var actual = Convert.ToInt32(text, 2);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("2")]   // 0と1以外
	[InlineData("a")]
	public void ToInt32_文字列を2進数としてInt32に変換できずFormatExceptionがスローされる(string text) {
		// Arrange
		// Act
		// Assert
		Assert.Throws<FormatException>(() => Convert.ToInt32(text, 2));
	}

	[Theory]
	[InlineData("1", 1)]
	[InlineData("01", 1)]   // 先頭が0でも変換できる
	[InlineData("10", 10)]
	public void ToInt32_文字列を10進数としてInt32に変換できる(string text, int expected) {
		// Arrange
		// Act
		var actual = Convert.ToInt32(text, 10);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("a")]
	public void ToInt32_文字列を10進数としてInt32に変換できずFormatExceptionがスローされる(string text) {
		// Arrange
		// Act
		// Assert
		Assert.Throws<FormatException>(() => Convert.ToInt32(text, 10));
	}

	[Theory]
	[InlineData("1", 1)]
	[InlineData("f", 15)]
	[InlineData("0f", 15)]  // 先頭が0でも変換できる
	[InlineData("FF", 255)] // 大文字でも変換できる
	public void ToInt32_文字列を16進数としてInt32に変換できる(string text, int expected) {
		// Arrange
		// Act
		var actual = Convert.ToInt32(text, 16);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("g")]
	public void ToInt32_文字列を16進数としてInt32に変換できずFormatExceptionがスローされる(string text) {
		// Arrange
		// Act
		// Assert
		Assert.Throws<FormatException>(() => Convert.ToInt32(text, 16));
	}

	[Theory]
	[InlineData(0b00000000, "0")]
	[InlineData(0b00000001, "1")]
	[InlineData(0b00000010, "10")]
	[InlineData(0b11110000, "11110000")]
	public void ToString_数値を2進数文字列に変換できる(int value, string expected) {
		// Arrange
		// Act
		var actual = Convert.ToString(value, 2);

		// Assert
		Assert.Equal(expected, actual);
	}

}
