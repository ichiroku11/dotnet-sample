using Xunit;
using Xunit.Abstractions;

namespace SampleTest;

public class ArgumentNullExceptionTest {
	private readonly ITestOutputHelper _output;

	public ArgumentNullExceptionTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void ThrowIfNull_引数がnullだと例外がスローされる() {
		// Arrange
		var value = default(string);
		// Act
		// Assert
		var exception = Assert.Throws<ArgumentNullException>(() => {
			ArgumentNullException.ThrowIfNull(null, nameof(value));
		});
		_output.WriteLine(exception.Message);
		Assert.Contains(nameof(value), exception.ParamName);
	}

	[Fact]
	public void ThrowIfNull_引数が非nullだと例外がスローされない() {
		// Arrange
		var value = "";

		// Act
		ArgumentNullException.ThrowIfNull(value, nameof(value));

		// Assert
		Assert.NotNull(value);
	}
}
