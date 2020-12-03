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
		private const string _input = @"xyz
abc
efg
ab c
xabc";

		// todo: 特定のキーワードを含む行を削除
		// todo: 特定のキーワードを含まない行を削除

		[Fact]
		public void 特定のキーワードを含む行の番号を列挙する() {
			// Arrange
			var regex = new Regex("^.*abc.*$");
			var results = new List<int>();

			// Act
			foreach (var (index, line) in _input.Split(Environment.NewLine).Select((line, index) => (index, line))) {
				var match = regex.Match(line);
				if (match.Success) {
					results.Add(index);
				}
			}

			// Assert
			Assert.Equal(new[] { 1, 4 }, results);
		}


		[Fact]
		public void 特定のキーワードを含まない行の番号を列挙する() {
			// Arrange
			var regex = new Regex("^(?!.*abc).*$");
			var results = new List<int>();

			// Act
			foreach (var (index, line) in _input.Split(Environment.NewLine).Select((line, index) => (index, line))) {
				var match = regex.Match(line);
				if (match.Success) {
					results.Add(index);
				}
			}

			// Assert
			Assert.Equal(new[] { 0, 2, 3 }, results);
		}
	}
}
