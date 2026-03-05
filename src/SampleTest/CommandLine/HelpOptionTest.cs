using System.CommandLine.Help;

namespace SampleTest.CommandLine;

public class HelpOptionTest {
	[Fact]
	public async Task Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		// Act
		var option = new HelpOption();

		// Assert
		Assert.IsType<HelpAction>(option.Action);
		Assert.Equal(["-h", "/h", "-?", "/?"], option.Aliases);
		Assert.Equal("--help", option.Name);
		Assert.True(option.Recursive);
		Assert.False(option.Required);
	}
}
