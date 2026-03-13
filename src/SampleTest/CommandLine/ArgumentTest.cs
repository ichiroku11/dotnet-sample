using System.CommandLine;

namespace SampleTest.CommandLine;

public class ArgumentTest {
	[Fact]
	public void DefaultValueFactory_string型のインスタンスの場合はnull() {
		// Arrange
		var argument = new Argument<string>("test") {
		};

		// Act
		// Assert
		Assert.Null(argument.DefaultValueFactory);
	}

	[Fact]
	public void DefaultValueFactory_int型のインスタンスの場合はnull() {
		// Arrange
		var argument = new Argument<int>("test") {
		};

		// Act
		// Assert
		Assert.Null(argument.DefaultValueFactory);
	}

	[Fact]
	public void DefaultValueFactory_bool型のインスタンスの場合は非null() {
		// Arrange
		var argument = new Argument<bool>("test") {
		};

		// Act
		// Assert
		// boolの場合は非null
		Assert.NotNull(argument.DefaultValueFactory);
	}
}
