using System.CommandLine.Parsing;

namespace SampleTest.CommandLine;

public class CommandLineParserTest {
	[Theory]
	[InlineData("a b c", new[] { "a", "b", "c" })]
	// 空白を含めたい場合は""で囲む
	[InlineData("a \"b c\"", new[] { "a", "b c" })]
	public void SplitCommandLine_文字列を分割する(string commandLine, string[] expected) {
		// Arrange
		// Act
		var actual = CommandLineParser.SplitCommandLine(commandLine);

		// Assert
		Assert.Equal(expected, actual);
	}
}
