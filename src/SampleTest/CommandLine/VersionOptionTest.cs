using System.CommandLine;

namespace SampleTest.CommandLine;

public class VersionOptionTest {
	[Fact]
	public async Task Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		// Act
		var option = new VersionOption();

		// Assert
		// アクションの型は公開されていない様子
		Assert.NotNull(option.Action);
		Assert.Equal("--version", option.Name);
		Assert.Empty(option.Aliases);
		Assert.False(option.Recursive);
		Assert.False(option.Required);
	}
}
