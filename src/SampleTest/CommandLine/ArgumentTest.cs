using System.CommandLine;

namespace SampleTest.CommandLine;

public class ArgumentTest {
	[Fact]
	public void Properties_string型のインスタンスのプロパティを確認する() {
		// Arrange
		var argument = new Argument<string>("test") {
		};

		// Act
		// Assert
		Assert.False(argument.HasDefaultValue);
		Assert.Null(argument.DefaultValueFactory);
	}

	[Fact]
	public void Properties_int型のインスタンスのプロパティを確認する() {
		// Arrange
		var argument = new Argument<int>("test") {
		};

		// Act
		// Assert
		Assert.False(argument.HasDefaultValue);
		Assert.Null(argument.DefaultValueFactory);
	}

	[Fact]
	public void Properties_bool型のインスタンスのプロパティを確認する() {
		// Arrange
		var argument = new Argument<bool>("test") {
		};

		// Act
		// Assert
		// boolの場合はデフォルト値がある
		Assert.True(argument.HasDefaultValue);
		// boolの場合は非null
		Assert.NotNull(argument.DefaultValueFactory);
	}
}
