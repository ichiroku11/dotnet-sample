namespace SampleTest.AspNetCore.Mvc.Testing;

public class CustomWebApplicationFactoryTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class EmptyProgram {
	}

	[Fact]
	public void CreateClient_やはり例外が発生する() {
		// Arrange
		using var factory = new CustomWebApplicationFactory<EmptyProgram>();

		// Act
		var exception = Record.Exception(() => factory.CreateClient());

		// Assert
		Assert.IsType<ArgumentException>(exception);
		// The content root '{パス}' does not exist. (Parameter 'contentRootPath')
		_output.WriteLine(exception.Message);
	}
}
