using System.CommandLine;
using System.CommandLine.Completions;
using System.CommandLine.Help;

namespace SampleTest.CommandLine;

public class RootCommandTest {
	[Fact]
	public void Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		// Act
		var command = new RootCommand();

		// Assert
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

	[Fact]
	public void Parse_ヘルプコマンドを実行した結果を確認する() {
		// Arrange
		var command = new RootCommand();

		// Act
		var result = command.Parse(["--help"]);

		// Assert
		Assert.Empty(result.Errors);
		Assert.IsType<HelpAction>(result.Action);
		Assert.Equal(["--help"], result.Tokens.Select(token => token.Value));
		Assert.Empty(result.UnmatchedTokens);
	}

	[Fact]
	public void Parse_バージョンコマンドを実行した結果を確認する() {
		// Arrange
		var command = new RootCommand();

		// Act
		var result = command.Parse(["--version"]);

		// Assert
		Assert.Empty(result.Errors);
		// アクションの型は公開されていない様子
		Assert.NotNull(result.Action);
		Assert.Equal(["--version"], result.Tokens.Select(token => token.Value));
		Assert.Empty(result.UnmatchedTokens);
	}
}
