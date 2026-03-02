using System.CommandLine;
using System.CommandLine.Completions;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace SampleTest.CommandLine;

public class RootCommandTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		// Act
		var command = new RootCommand();

		// Assert
		Assert.Null(command.Action);
		Assert.Empty(command.Aliases);
		Assert.Empty(command.Arguments);
		Assert.NotNull(command.Description);
		Assert.Empty(command.Description);

		var directive = Assert.Single(command.Directives);
		Assert.IsType<SuggestDirective>(directive);

		Assert.Equal(2, command.Options.Count);
		Assert.Single(command.Options.OfType<HelpOption>());
		Assert.Single(command.Options.OfType<VersionOption>());

		Assert.Empty(command.Subcommands);
	}

	[Theory]
	[InlineData("--help")]
	[InlineData("-h")]
	[InlineData("-?")]
	public void Parse_helpオプションを指定した結果を確認する(string arg) {
		// Arrange
		var command = new RootCommand();

		// Act
		var result = command.Parse([arg]);

		// Assert
		Assert.Empty(result.Errors);

		Assert.IsType<HelpAction>(result.Action);

		var token = Assert.Single(result.Tokens);
		Assert.Equal(TokenType.Option, token.Type);
		Assert.Equal(arg, token.Value);
		Assert.Empty(result.UnmatchedTokens);
	}

	[Fact]
	public void Parse_versionオプションを指定した結果を確認する() {
		// Arrange
		var command = new RootCommand();

		// Act
		// --versionのみ、-vなどのオプションはない様子
		var result = command.Parse(["--version"]);

		// Assert
		Assert.Empty(result.Errors);

		// アクションの型は公開されていない様子
		Assert.NotNull(result.Action);

		var token = Assert.Single(result.Tokens);
		Assert.Equal(TokenType.Option, token.Type);
		Assert.Equal("--version", token.Value);
		Assert.Empty(result.UnmatchedTokens);
	}

	[Fact]
	public void Parse_空文字列の配列で呼び出した場合はエラーになる() {
		// Arrange
		var command = new RootCommand();

		// Act
		var result = command.Parse([""]);

		// Assert
		Assert.IsType<ParseErrorAction>(result.Action);

		var error = Assert.Single(result.Errors);
		Assert.NotEmpty(error.Message);
		Assert.NotNull(error.SymbolResult);
		_output.WriteLine(error.Message);

		var token = Assert.Single(result.Tokens);
		Assert.Equal(TokenType.Argument, token.Type);
		Assert.Equal("", token.Value);

		var unmatchedToken = Assert.Single(result.UnmatchedTokens);
		Assert.Equal("", unmatchedToken);
	}
}
