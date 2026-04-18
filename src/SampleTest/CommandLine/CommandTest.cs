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

		// Act
		var result = command.Parse(args);

		// Assert
		Assert.False(result.Errors.Any());
	}

	[Fact]
	public void Parse_int配列型のオプションに対してオプション名を複数指定しない場合はエラーが発生する() {
		// Arrange
		var option = new Option<int[]>("--value");
		var command = new Command("test") {
			option,
		};

		// Act
		var result = command.Parse(["test", "--value", "2", "1"]);

		// Assert
		var error = Assert.Single(result.Errors);
		_output.WriteLine(error.Message);
		// Unrecognized command or argument '1'.
	}

	[Fact]
	public void Parse_int配列型のオプションに対して複数の引数を指定するにはオプション名を繰り返して指定する() {
		// Arrange
		var option = new Option<int[]>("--value");
		var command = new Command("test") {
			option,
		};

		// Act
		var result = command.Parse(["test", "--value", "2", "--value", "1"]);

		// Assert
		Assert.Empty(result.Errors);

		var actual = result.GetValue(option);
		Assert.NotNull(actual);
		Assert.Equal([2, 1], actual);
	}

	[Fact]
	public void Parse_int配列型のオプションに対してAllowMultipleArgumentsPerTokenを使って複数の引数を指定する() {
		// Arrange
		var option = new Option<int[]>("--value") {
			AllowMultipleArgumentsPerToken = true,
		};
		var command = new Command("test") {
			option,
		};

		// Act
		var result = command.Parse(["test", "--value", "2", "1"]);

		// Assert
		Assert.Empty(result.Errors);

		var actual = result.GetValue(option);
		Assert.NotNull(actual);
		Assert.Equal([2, 1], actual);
	}

	[Theory]
	[InlineData("yyyy-MM-dd")]
	[InlineData("yyyy/MM/dd")]
	[InlineData("yyyy.MM.dd")]
	public void Parse_DateOnly型のオプションに対して日付文字列を指定できる(string format) {
		// Arrange
		var option = new Option<DateOnly>("--date");
		var command = new Command("test") {
			option
		};

		var expected = DateOnly.FromDateTime(DateTime.Today);

		var arg = expected.ToString(format);
		_output.WriteLine(arg);

		// Act
		var result = command.Parse(["test", "--date", arg]);

		// Assert
		Assert.Empty(result.Errors);
		Assert.Equal(expected, result.GetValue(option));
	}

	[Theory]
	[InlineData("1", DayOfWeek.Monday)]
	[InlineData("monday", DayOfWeek.Monday)]
	[InlineData("Monday", DayOfWeek.Monday)]
	public void Parse_enumのDayOfWeek型のオプションに対して文字列を指定できる(string arg, DayOfWeek expected) {
		// Arrange
		var option = new Option<DayOfWeek>("--dayofweek");
		var command = new Command("test") {
			option
		};

		// Act
		var result = command.Parse(["test", "--dayofweek", arg]);

		// Assert
		Assert.Empty(result.Errors);
		Assert.Equal(expected, result.GetValue(option));
	}

	[Theory]
	[InlineData("-v")]
	[InlineData("/v")]
	public void Parse_エイリアスを指定してオプションを取得できる(string alias) {
		// Arrange
		var option = new Option<string>("--value", "-v", "/v");
		var command = new Command("test") {
			option
		};

		// Act
		var result = command.Parse(["test", alias, "hello"]);

		// Assert
		Assert.Empty(result.Errors);
		Assert.Equal("hello", result.GetValue(option));
	}

	[Fact]
	public void Parse_コマンド名は大文字小文字を区別するため一致しない場合はエラーが発生する() {
		// Arrange
		var command = new Command("test") {
		};

		// Act
		// 指定するCommand名が大文字で始まっている
		var result = command.Parse(["Test"]);

		// Assert
		var error = Assert.Single(result.Errors);
		_output.WriteLine(error.Message);
		// Unrecognized command or argument 'Test'.
	}

	[Fact]
	public void Parse_オプション名は大文字小文字を区別するため一致しない場合はエラーが発生する() {
		// Arrange
		var command = new Command("test") {
			new Option<string>("--value")
		};

		// Act
		// 指定するOption名が大文字で始まっている
		var result = command.Parse(["test", "--Value"]);

		// Assert
		var error = Assert.Single(result.Errors);
		_output.WriteLine(error.Message);
		// Unrecognized command or argument '--Value'.
	}
}
