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
		Assert.Equal(ArgumentArity.ExactlyOne, option.Arity);
		Assert.False(option.HasDefaultValue);
		Assert.Null(option.DefaultValueFactory);
		Assert.False(option.Recursive);
		Assert.False(option.Required);
	}
}
