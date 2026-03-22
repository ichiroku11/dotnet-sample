using System.CommandLine;

namespace SampleTest.CommandLine;

public class ParseResultTest {
	[Fact]
	public void Action_コマンドに指定したアクションと同じインスタンスを取得できる() {
		// Arrange
		var command = new Command("test");
		command.SetAction(action);

		var result = command.Parse([]);
		var code = result.Invoke();

		// Act
		// Assert
		Assert.Same(command.Action, result.Action);
		Assert.Equal(0, code);

		static void action(ParseResult result) {
		}
	}

	[Fact]
	public void CommandResult_サブコマンドを指定しない場合はParseしたコマンドになることを確認する() {
		// Arrange
		var command = new Command("main") {
		};

		// Act
		var result = command.Parse(["main"]);

		// Assert
		Assert.Empty(result.Errors);
		Assert.Same(command, result.CommandResult.Command);
	}

	[Fact]
	public void CommandResult_サブコマンドを指定した場合はサブコマンドになることを確認する() {
		// Arrange
		var subCommand = new Command("sub");
		var mainCommand = new Command("main") {
			subCommand
		};

		// Act
		var result = mainCommand.Parse(["main", "sub"]);

		// Assert
		Assert.Empty(result.Errors);
		Assert.Same(subCommand, result.CommandResult.Command);
	}

	[Fact]
	public void Invoke_コマンドに指定したアクションが呼び出される() {
		// Arrange
		var done = false;
		var command = new Command("test");
		// アクションを指定する
		command.SetAction(result => {
			done = true;
		});

		var result = command.Parse([]);

		// Act
		var code = result.Invoke();

		// Assert
		Assert.True(done);
		Assert.Equal(0, code);
	}

	[Fact]
	public void Invoke_コマンドにアクションを指定しなくてもエラーにならない() {
		// Arrange
		var command = new Command("test");
		var result = command.Parse([]);

		// Act
		var code = result.Invoke();

		// Assert
		Assert.Null(result.Action);
		Assert.Equal(0, code);
	}

	[Theory]
	// オプションを指定しない
	[InlineData(new string[] { }, false)]
	// オプションを指定し、オプション引数を省略する
	[InlineData(new string[] { "--test" }, true)]
	// オプションとオプション引数を指定する
	[InlineData(new string[] { "--test", "true" }, true)]
	[InlineData(new string[] { "--test", "false" }, false)]
	public void GetValue_bool型のオプションの値を確認する(string[] args, bool expected) {
		// Arrange
		var option = new Option<bool>("--test");
		var command = new Command("test") {
			option,
		};

		var result = command.Parse(args);
		var code = result.Invoke();

		// Act
		var actual = result.GetValue(option);

		// Assert
		Assert.Equal(0, code);
		Assert.Equal(expected, actual);
	}

	[Theory]
	// オプションを指定しない => デフォルト値が取得できる？
	[InlineData(new string[] { }, 0)]
	// オプションとオプション引数を指定する
	[InlineData(new string[] { "--test", "1" }, 1)]
	public void GetValue_int型のオプションの値を確認する(string[] args, int expected) {
		// Arrange
		var option = new Option<int>("--test");
		var command = new Command("test") {
			option,
		};
		command.Options.Add(option);

		var result = command.Parse(args);
		var code = result.Invoke();

		// Act
		var actual = result.GetValue(option);

		// Assert
		Assert.Equal(0, code);
		Assert.Equal(expected, actual);
	}
}
