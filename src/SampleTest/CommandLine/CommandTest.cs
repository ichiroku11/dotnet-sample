using System.CommandLine;

namespace SampleTest.CommandLine;

public class CommandTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Parse_サブコマンドが存在する場合はサブコマンドを指定しないとエラーになる() {
		// Arrange
		var command = new Command("main") {
			new Command("sub")
		};

		// Act
		var result = command.Parse(["main"]);

		// Assert
		Assert.NotEmpty(result.Errors);
		foreach (var error in result.Errors) {
			_output.WriteLine(error.Message);
		}
	}

	[Theory]
	// オプション引数がboolではない場合
	[InlineData(["--test", "1"])]
	public void Parse_bool型のオプションに対してエラーが発生するコマンドライン引数を確認する(params string[] args) {
		// Arrange
		var option = new Option<bool>("--test");
		var command = new Command("test") {
			option,
		};
		command.Options.Add(option);

		// Act
		var result = command.Parse(args);

		// Assert
		Assert.True(result.Errors.Any());
		foreach (var error in result.Errors) {
			_output.WriteLine(error.Message);
		}
	}

	[Theory]
	// オプションを指定しない
	[InlineData([])]
	// boolの場合だけ特殊？
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

	[Theory]
	// オプションを指定し、オプション引数を省略する
	[InlineData(["--test"])]
	// オプション引数が数字ではない場合
	[InlineData(["--test", "a"])]
	public void Parse_int型のオプションに対してエラーが発生するコマンドライン引数を確認する(params string[] args) {
		// Arrange
		var option = new Option<int>("--test");
		var command = new Command("test") {
			option,
		};
		command.Options.Add(option);

		// Act
		var result = command.Parse(args);

		// Assert
		Assert.True(result.Errors.Any());
		foreach (var error in result.Errors) {
			_output.WriteLine(error.Message);
		}
	}

	[Theory]
	// オプションを指定しない
	[InlineData([])]
	// オプションとオプション引数を指定する
	[InlineData(["--test", "1"])]
	public void Parse_int型のオプションに対してエラーが発生しないコマンドライン引数を確認する(params string[] args) {
		// Arrange
		var option = new Option<int>("--test");
		var command = new Command("test") {
			option,
		};
		command.Options.Add(option);

		// Act
		var result = command.Parse(args);

		// Assert
		Assert.False(result.Errors.Any());
	}
}
