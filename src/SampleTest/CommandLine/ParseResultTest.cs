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
		var invoked = false;
		var command = new Command("test");
		// アクションを指定する
		command.SetAction(result => {
			invoked = true;
		});

		var result = command.Parse([]);

		// Act
		var code = result.Invoke();

		// Assert
		Assert.True(invoked);
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
}
