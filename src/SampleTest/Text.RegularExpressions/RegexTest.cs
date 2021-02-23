using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.RegularExpressions {
	public class RegexTest {
		[Fact]
		public void 特定のキーワードを含む行の番号を列挙する() {
			// Arrange
			var regex = new Regex("^.*abc.*$");
			const string input = @"xyz
abc
efg
ab c
xabc";
			var results = new List<int>();

			// Act
			foreach (var (index, line) in input.Split(Environment.NewLine).Select((line, index) => (index, line))) {
				var match = regex.Match(line);
				if (match.Success) {
					results.Add(index);
				}
			}

			// Assert
			Assert.Equal(new[] { 1, 4 }, results);
		}

		[Fact]
		public void 特定のキーワードを含む行を削除する() {
			// Arrange
			var regex = new Regex(@"^.*abc.*$(\n|\r\n)*", RegexOptions.Multiline);
			const string input = @"xyz
abc
efg
ab c
xabc";
			// Act
			var actual = regex.Replace(input, "");

			// Assert
			const string expected = @"xyz
efg
ab c
";
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void 特定のキーワードを含まない行の番号を列挙する() {
			// Arrange
			var regex = new Regex("^(?!.*abc).*$");
			const string input = @"xyz
abc
efg
ab c
xabc";
			var results = new List<int>();

			// Act
			foreach (var (index, line) in input.Split(Environment.NewLine).Select((line, index) => (index, line))) {
				var match = regex.Match(line);
				if (match.Success) {
					results.Add(index);
				}
			}

			// Assert
			Assert.Equal(new[] { 0, 2, 3 }, results);
		}

		[Fact]
		public void 特定のキーワードを含まない行を削除する() {
			// Arrange
			var regex = new Regex("^(?!.*abc).*$(\n|\r\n)*", RegexOptions.Multiline);
			const string input = @"xyz
abc
efg
ab c
xabc";
			// Act
			var actual = regex.Replace(input, "");

			// Assert
			const string expected = @"abc
xabc";
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("0", true)]
		[InlineData("0123456789", true)]
		[InlineData("", false)]
		[InlineData("0a", false)]
		// \dは全角の数字も数字と判定する
		[InlineData("０１２３４５６７８９", true)]
		public void 文字列が数字だけで構成されているかどうかを判定する正規表現(string input, bool expected) {
			// Arrange
			var regex = new Regex(@"^\d+$");

			// Act
			var actual = regex.IsMatch(input);

			// Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("0", true)]
		[InlineData("0123456789", true)]
		[InlineData("", false)]
		[InlineData("0a", false)]
		[InlineData("０１２３４５６７８９", false)]
		public void 文字列が半角数字だけで構成されているかどうかを判定する正規表現(string input, bool expected) {
			// Arrange
			var regex = new Regex("^[0-9]+$");

			// Act
			var actual = regex.IsMatch(input);

			// Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("a", true)]
		[InlineData("abcdefghijklmnopqrstuvwxyz", true)]
		[InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", true)]
		[InlineData("", false)]
		[InlineData("0a", false)]
		// 全角の英字
		[InlineData("ａ", false)]
		public void 文字列が半角英字だけで構成されているかどうかを判定する正規表現(string input, bool expected) {
			// Arrange
			var regex = new Regex("^[a-zA-Z]+$");

			// Act
			var actual = regex.IsMatch(input);

			// Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("-", true)]
		[InlineData("-aA", true)]
		public void 文字列がハイフンを含めた半角英字で構成されているかどうかを判定する正規表現(string input, bool expected) {
			// Arrange
			// "-"自体を含めたい場合は、"[]"内の先頭か末尾に"-"を指定する
			var regex = new Regex("^[a-zA-Z-]+$");

			// Act
			var actual = regex.IsMatch(input);

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
