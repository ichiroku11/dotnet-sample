using System.CommandLine;

namespace SampleTest.CommandLine;

public class CommandTest {
	[Theory]
	// オプションを指定しない
	[InlineData([])]
	// オプションを指定し、オプション引数を省略する
	[InlineData(["--test"])]
	// オプションとオプション引数を指定する
	[InlineData(["--test", "true"])]
	[InlineData(["--test", "false"])]
	// 区切り文字として"="と":"も有効
	// https://learn.microsoft.com/ja-jp/dotnet/standard/commandline/syntax#option-argument-delimiters
	[InlineData(["--test=true"])]
	[InlineData(["--test=false"])]
	[InlineData(["--test:true"])]
	[InlineData(["--test:false"])]
	public void Parse_bool型のオプションに対してエラーが発生しないコマンドライン引数を確認する(params string[] args) {
		// Arrange
		var option = new Option<bool>("--test");
		var command = new Command("test") {
			option,
		};
		command.Options.Add(option);

		// Act
		var result = command.Parse(args);

		// Assert
		Assert.False(result.Errors.Any());
	}

	// todo: int
}
