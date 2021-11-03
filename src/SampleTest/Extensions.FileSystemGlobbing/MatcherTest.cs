using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Extensions.FileSystemGlobbing {
	// https://docs.microsoft.com/ja-jp/dotnet/core/extensions/file-globbing
	public class MatcherTest {
		[Fact]
		public void Match_マッチが見つかる単純動きを確認する() {
			// Arrange
			var matcher = new Matcher();
			// 含めるパターン指定する
			matcher.AddInclude("*.md");

			// Act
			// 指定したパスがマッチするかどうかを確認する
			var result = matcher.Match("readme.md");

			// Assert
			Assert.IsType<PatternMatchingResult>(result);
			Assert.True(result.HasMatches);

			var match = Assert.Single(result.Files);
			Assert.IsType<FilePatternMatch>(match);
			Assert.Equal("readme.md", match.Path);
		}
	}
}
