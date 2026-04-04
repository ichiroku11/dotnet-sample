using System.CommandLine;

namespace SampleTest.CommandLine;

public class OptionTest {
	[Fact]
	public void Properties_bool型のインスタンスのプロパティを確認する() {
		// Arrange
		var option = new Option<bool>("test") {
		};

		// Act
		// Assert
		Assert.Equal("test", option.Name);
		Assert.False(option.AllowMultipleArgumentsPerToken);
		// boolの場合は0または1
		Assert.Equal(ArgumentArity.ZeroOrOne, option.Arity);
		// boolの場合はデフォルト値がある
		Assert.True(option.HasDefaultValue);
		// boolの場合は非null
		Assert.NotNull(option.DefaultValueFactory);

		Assert.False(option.Recursive);
		Assert.False(option.Required);
	}

	[Fact]
	public void Properties_int型のインスタンスのプロパティを確認する() {
		// Arrange
		var option = new Option<int>("test") {
		};

		// Act
		// Assert
		Assert.Equal("test", option.Name);
		Assert.False(option.AllowMultipleArgumentsPerToken);
		Assert.Equal(ArgumentArity.ExactlyOne, option.Arity);
		Assert.False(option.HasDefaultValue);
		Assert.Null(option.DefaultValueFactory);
		Assert.False(option.Recursive);
		Assert.False(option.Required);

	}

	[Fact]
	public void Properties_string型のインスタンスのプロパティを確認する() {
		// Arrange
		var option = new Option<string>("test") {
		};

		// Act
		// Assert
		Assert.Equal("test", option.Name);
		Assert.False(option.AllowMultipleArgumentsPerToken);
		Assert.Equal(ArgumentArity.ExactlyOne, option.Arity);
		Assert.False(option.HasDefaultValue);
		Assert.Null(option.DefaultValueFactory);
		Assert.False(option.Recursive);
		Assert.False(option.Required);
	}

	[Fact]
	public void DefaultValueFactory_オプションを指定しない場合はParseのタイミングで呼び出される() {
		// Arrange
		var called = false;
		var option = new Option<int>("--test") {
			DefaultValueFactory = result => {
				// Command.Parseのタイミングで呼び出される
				called = true;
				return 99;
			},

		};
		var command = new Command("test") {
			option
		};

		// Act
		// Assert
		// Parse前は呼び出されない
		Assert.False(called);

		var result = command.Parse(["test"]);

		// Parseで呼び出される
		Assert.True(called);

		Assert.Equal(99, result.GetValue(option));
	}

	[Fact]
	public void DefaultValueFactory_オプションを指定した場合は呼び出されない() {
		// Arrange
		var called = false;
		var option = new Option<int>("--test") {
			DefaultValueFactory = result => {
				// オプションを指定した場合は呼び出されない
				Assert.Fail();

				called = true;
				return 99;
			},
		};
		var command = new Command("test") {
			option
		};

		// Act
		var result = command.Parse(["test", "--test", "100"]);

		// Assert
		// Parseで呼び出されない
		Assert.False(called);

		// コマンドライン引数に指定した値が取得できる
		Assert.Equal(100, result.GetValue(option));
	}
}
