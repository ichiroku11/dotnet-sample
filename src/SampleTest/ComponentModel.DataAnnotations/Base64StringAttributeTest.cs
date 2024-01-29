using System.ComponentModel.DataAnnotations;

namespace SampleTest.ComponentModel.DataAnnotations;

public class Base64StringAttributeTest {
	[Theory]
	[InlineData(null, true)]
	// 文字列以外はfalseになる
	[InlineData(false, false)]
	[InlineData(0, false)]
	// 空文字はtrueになる
	[InlineData("", true)]
	// 文字列"abcd"
	[InlineData("YWJjZA==", true)]
	// 内部的にはConvert.TryFromBase64Stringが使われている？
	// https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.base64stringattribute?view=net-8.0
	// Base64に使われる文字がtrueというわけではない
	[InlineData("=", false)]
	public void IsValid_確認する(object? value, bool expected) {
		// Arrange
		var attribute = new Base64StringAttribute();

		// Act
		var actual = attribute.IsValid(value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
