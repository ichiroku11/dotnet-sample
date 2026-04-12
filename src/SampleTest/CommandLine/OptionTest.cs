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
		Assert.Null(option.CustomParser);
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
		Assert.Null(option.CustomParser);
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
		Assert.Null(option.CustomParser);
		Assert.False(option.HasDefaultValue);
		Assert.Null(option.DefaultValueFactory);
		Assert.False(option.Recursive);
		Assert.False(option.Required);
	}

	[Fact]
	public void Properties_int型配列のインスタンスのプロパティを確認する() {
		// Arrange
		var option = new Option<int[]>("test") {
		};

		// Act
		// Assert
		Assert.Equal("test", option.Name);
		Assert.False(option.AllowMultipleArgumentsPerToken);
		// 1以上
		Assert.Equal(ArgumentArity.OneOrMore, option.Arity);
		Assert.Null(option.CustomParser);
		Assert.False(option.HasDefaultValue);
		Assert.Null(option.DefaultValueFactory);
		Assert.False(option.Recursive);
		Assert.False(option.Required);
	}

	private record struct Vector(int X, int Y) {
		public static readonly Vector Empty = new();
	}

	[Fact]
	public void CustomParser_2つの引数から独自の型を受け取るオプションを作ってみる() {
		// Arrange
		var option = new Option<Vector>("--vector") {
			// 複数の引数を受け取る
			AllowMultipleArgumentsPerToken = true,
			// ちょうど2つの引数を受け取る
			Arity = new ArgumentArity(2, 2),
			// 独自の型に変換する
			CustomParser = result => {
				// エラー処理は省略

				var x = int.Parse(result.Tokens[0].Value);
				var y = int.Parse(result.Tokens[^1].Value);

				return new Vector(x, y);
			},
		};
		var command = new Command("test") {
			option
		};

		// Act
		var result = command.Parse(["test", "--vector", "1", "2"]);

		// Assert
		Assert.Empty(result.Errors);
		Assert.Equal(new Vector(1, 2), result.GetValue(option));
	}

	[Fact]
	public void CustomParser_エラーが発生する場合の動きを確認する() {
		// Arrange
		var option = new Option<Vector>("--test") {
			CustomParser = result => {
				result.AddError("--test error");
				return Vector.Empty;
			},
		};
		var command = new Command("test") {
			option
		};

		// Act
		var result = command.Parse(["test", "--test", "1"]);

		// Assert
		var error = Assert.Single(result.Errors);
		Assert.Equal("--test error", error.Message);
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
