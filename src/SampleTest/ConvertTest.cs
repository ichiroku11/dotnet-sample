using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	public class ConvertTest {
		[Theory]
		[InlineData("1", 1)]
		[InlineData("0001", 1)]	// 先頭に0があっても大丈夫
		[InlineData("11111111", 255)]
		public void ToInt32_文字列を2進数としてInt32に変換できる(string text, int expected) {
			// Arrange
			// Act
			var actual = Convert.ToInt32(text, 2);

			// Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("2")]	// 0と1以外
		[InlineData("a")]
		public void ToInt32_文字列を2進数としてInt32に変換できずFormatExceptionがスローされる(string text) {
			// Arrange
			// Act
			// Assert
			Assert.Throws<FormatException>(() => Convert.ToInt32(text, 2));
		}

		[Theory]
		[InlineData("1", 1)]
		[InlineData("01", 1)]	// 先頭が0でも変換できる
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
		[InlineData("0f", 15)]	// 先頭が0でも変換できる
		[InlineData("FF", 255)]	// 大文字でも変換できる
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
	}
}
