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
		public void Match_マッチが1つ見つかる動きを確認する() {
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

		[Fact]
		public void Match_マッチが見つからない動きを確認する() {
			// Arrange
			var matcher = new Matcher();
			matcher.AddInclude("*.md");

			// Act
			var result = matcher.Match("readme.txt");

			// Assert
			Assert.False(result.HasMatches);
			Assert.Empty(result.Files);
		}

		[Theory]
		[InlineData("readme.md")]
		[InlineData("folder/readme.md")]
		[InlineData("folder/subfolder/readme.md")]
		public void Match_globstarで任意のディレクトリ内にあるファイルが見つかる動きを確認する(string file) {
			// Arrange
			var matcher = new Matcher();
			matcher.AddInclude("**/*.md");

			// Act
			var result = matcher.Match(file);

			// Assert
			Assert.True(result.HasMatches);
			var match = Assert.Single(result.Files);
			Assert.Equal(file, match.Path);
		}

		[Theory]
		[InlineData("readme.md", false)]
		[InlineData("folder/readme.md", true)]
		[InlineData("folder/subfolder/readme.md", false)]
		public void Match_ワイルドカードでサブディレクトリ内にあるファイルが見つかる動きを確認する(string file, bool expected) {
			// Arrange
			var matcher = new Matcher();
			matcher.AddInclude("*/*.md");

			// Act
			var actual = matcher.Match(file).HasMatches;

			// Assert
			Assert.Equal(expected, actual);
		}

		// FilePatternMatch.Path - パターンの開始を基準とした相対パス
		// FilePatternMatch.Stem - パターンのワイルド―カード部分を基準とした相対パス
		// https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.FileSystemGlobbing/src/FilePatternMatch.cs
		[Theory]
		// globstar
		[InlineData("src/Project/**/*.cs", "src/Project/File.cs", "File.cs")]
		[InlineData("src/Project/**/*.cs", "src/Project/Interfaces/IFile.cs", "Interfaces/IFile.cs")]
		[InlineData("src/Project/**/*.cs", "src/Project/Interfaces/Files/IFile.cs", "Interfaces/Files/IFile.cs")]
		// wildcard
		[InlineData("src/Project/*/*.cs", "src/Project/Interfaces/IFile.cs", "Interfaces/IFile.cs")]
		public void Match_FilePatternMatchのPathとStemを確認する(string pattern, string file, string expectedStem) {
			// Arrange
			var matcher = new Matcher();
			matcher.AddInclude(pattern);

			// Act
			var result = matcher.Match(file);

			// Assert
			Assert.True(result.HasMatches);
			var match = Assert.Single(result.Files);
			Assert.Equal(file, match.Path);
			Assert.Equal(expectedStem, match.Stem);
		}
	}
}
