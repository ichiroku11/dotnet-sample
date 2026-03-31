using System.CommandLine;

namespace SampleTest.CommandLine;

public class InvocationConfigurationTest {
	[Fact]
	public void Output_Error_標準出力と標準エラーをカスタマイズできる() {
		// Arrange
		var command = new Command("test") {
		};
		command.SetAction(result => {
			// 標準出力と標準エラーに書き込む
			var output = result.InvocationConfiguration.Output;
			output.Write("This is output.");

			var error = result.InvocationConfiguration.Error;
			error.Write("This is error.");
		});

		// 標準出力と標準エラーをカスタマイズする
		using var output = new StringWriter();
		using var error = new StringWriter();
		var config = new InvocationConfiguration {
			Output = output,
			Error = error,
		};

		// Act
		command.Parse(["test"]).Invoke(config);

		// Assert
		Assert.Equal("This is output.", output.ToString());
		Assert.Equal("This is error.", error.ToString());
	}
}
