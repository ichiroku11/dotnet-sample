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
	}
}
