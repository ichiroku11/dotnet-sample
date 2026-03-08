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
	public void GetValue_bool型のオプションを取得できることを確認する(string[] args, bool expected) {
		// Arrange
		var option = new Option<bool>("--test");
		var command = new Command("test") {
			option,
		};

		// Act
		var actual = false;
		command.SetAction(result => {
			actual = result.GetValue(option);
		});
		var result = command.Parse(args);
		var code = result.Invoke();

		// Assert
		Assert.Equal(expected, actual);
		Assert.Equal(0, code);
	}

	// todo: int
}
