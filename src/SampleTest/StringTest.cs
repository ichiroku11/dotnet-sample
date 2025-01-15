using System.Text;

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
	// 全角英数字
	[InlineData("０", false)]
	[InlineData("ａ", false)]
	[InlineData("Ａ", false)]
	// 半角カタカナ
	[InlineData("ｱ", false)]
	// 囲み数字
	[InlineData("①", false)]
	// 
	[InlineData("㍍", false)]
	public void IsNormalized_FormKC_Unicode正規形の文字列かどうかを判定できる(string src, bool expected) {
		// Arrange
		// Act
		var actual = src.IsNormalized(NormalizationForm.FormKC);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Theory]
	// 全角英数字 => 半角英数字
	[InlineData("０１２３４", "01234")]
	[InlineData("ａｂｃｄｅ", "abcde")]
	[InlineData("ＡＢＣＤＥ", "ABCDE")]
	// 半角カタカナ => 全角カタカナ
	[InlineData("ｱｲｳｴｵ", "アイウエオ")]
	// 囲み数字 => 半角数字
	[InlineData("①②③", "123")]
	// 
	[InlineData("㍍", "メートル")]
	public void Normalize_FormKC_Unicode正規形の文字列を取得する(string src, string expected) {
		// Arrange
		// Act
		var actual = src.Normalize(NormalizationForm.FormKC);

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

	// 空文字列の場合
	public static TheoryData<string, string, StringSplitOptions, string[]> GetTheoryData_Split_Empty() {
		const string source = "";
		const string separator = ",";

		return new() {
			{ source, separator, StringSplitOptions.None, new[] { "" } },
			{ source, separator, StringSplitOptions.RemoveEmptyEntries, Array.Empty<string>() },
			{ source, separator, StringSplitOptions.TrimEntries, new[] { "" } },
			{ source, separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries, Array.Empty<string>() },
		};
	}

	// 空白の場合
	public static TheoryData<string, string, StringSplitOptions, string[]> GetTheoryData_Split_WhiteSpace() {
		const string source = " ";
		const string separator = ",";

		return new() {
			{ source, separator, StringSplitOptions.None, new[] { " " } },
			{ source, separator, StringSplitOptions.RemoveEmptyEntries, new[] { " " } },
			{ source, separator, StringSplitOptions.TrimEntries, new[] { "" } },
			{ source, separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries, Array.Empty<string>() },
		};
	}

	// 分割すると空文字や空白を含む場合
	public static TheoryData<string, string, StringSplitOptions, string[]> GetTheoryData_Split() {
		const string source = "a,b,, c ,";
		const string separator = ",";

		return new() {
			// 空白がそのまま残る
			{ source, separator, StringSplitOptions.None, new[] { "a", "b", "", " c ", "" } },
			// 空白だけの文字列が削除される
			{ source, separator, StringSplitOptions.RemoveEmptyEntries, new[] { "a", "b", " c " } },
			// 各文字列がTrimされる
			{ source, separator, StringSplitOptions.TrimEntries, new[] { "a", "b", "", "c", "" } },
			// 空白だけの文字列が削除される、かつ、各文字列がTrimされる
			{ source, separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries, new[] { "a", "b", "c" } },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Split_Empty))]
	[MemberData(nameof(GetTheoryData_Split_WhiteSpace))]
	[MemberData(nameof(GetTheoryData_Split))]
	public void Split_StringSplitOptionsの違いを確認する(string source, string separator, StringSplitOptions options, string[] expected) {
		// Arrange
		// Act
		var actual = source.Split(separator, options);

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

	[Theory]
	// 末尾に対象の文字が複数表れる場合もすべて削除される
	[InlineData("abc_-", new[] { '-', '_' }, "abc")]
	[InlineData("ab-_c_-", new[] { '-', '_' }, "ab-_c")]
	public void TrimEnd_複数charを渡す場合も末尾の文字列を削除できる(string src, char[] trimChars, string expected) {
		// Arrange
		// Act
		var actual = src.TrimEnd(trimChars);

		// Assert
		Assert.Equal(expected, actual);
	}
}
