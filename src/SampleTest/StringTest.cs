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

	// https://learn.microsoft.com/ja-jp/dotnet/api/system.string.trimend?view=net-8.0
	[Theory]
	[InlineData("abc-", '-', "abc")]
	// 末尾に対象の文字が複数ある場合もすべて削除される
	[InlineData("abc--", '-', "abc")]
	[InlineData("ab-c--", '-', "ab-c")]
	public void TrimEnd_末尾の文字列を削除できる(string src, char trimChar, string expected) {
		// Arrange
		// Act
		var actual = src.TrimEnd(trimChar);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void TrimEnd_メソッドを呼び出したインスタンスは変更されない() {
		// Arrange
		var src = "abc-";

		// Act
		var _ = src.TrimEnd('-');

		// Assert
		Assert.Equal("abc-", src);
	}

	[Fact]
	public void TrimEnd_トリミングできない場合の戻り値は同じインスタンス() {
		// Arrange
		var src = "abc";

		// Act
		var actual = src.TrimEnd('-');

		// Assert
		Assert.Same(src, actual);
	}

	// todo: 複数のcharを渡す場合
}
