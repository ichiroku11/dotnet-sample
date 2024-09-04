namespace SampleTest.Xunit;

// 文字列に対するAssertion
public class AssertForStringTest {
	[Fact]
	public void Contains_文字列が含まれていることを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.Contains("cde", "abcdefg");
	}

	[Fact]
	public void DoesNotContain_文字列が含まれていないことを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.DoesNotContain("fgh", "abcdefg");
	}

	[Fact]
	public void Empty_文字列が空であることを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.Empty("");
	}

	[Fact]
	public void NotEmpty_文字列が空でないことを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.NotEmpty("a");
		Assert.NotEmpty(" ");
	}

	[Fact]
	public void Equal_文字列が等しいことを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.Equal("x", "x");

		// 大文字小文字の違いを無視する
		Assert.Equal("x", "X", ignoreCase: true);

		// 空白を無視する
		Assert.Equal("xy", " x y", ignoreAllWhiteSpace: true);
		// 行末の空白は無視できない？
		// Assert.Equal("xy", " x y ", ignoreAllWhiteSpace: true);
	}

	[Fact]
	public void NotEqual_文字列が等しくないことを検証する() {
		// Arrange
		// Act
		// Assert

		// 大文字小文字の違い等しくない
		Assert.NotEqual("x", "X");
	}

	[Fact]
	public void StartsWith_文字列が指定した部分文字列で始まっていることを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.StartsWith("abc", "abcdefg");
	}

	[Fact]
	public void EndsWith_文字列が指定した部分文字列で終わっていることを検証する() {
		// Arrange
		// Act
		// Assert
		Assert.EndsWith("efg", "abcdefg");
	}
}
