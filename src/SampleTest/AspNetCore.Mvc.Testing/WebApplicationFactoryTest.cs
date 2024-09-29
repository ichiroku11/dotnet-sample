using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SampleTest.AspNetCore.Mvc.Testing;


public class WebApplicationFactoryTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class EmptyProgram {
	}

	[Fact]
	public void CreateClient_TEntryPointに空のクラスを指定すると例外が発生する() {
		// Arrange
		using var factory = new WebApplicationFactory<EmptyProgram>();

		// Act
		var exception = Record.Exception(() => factory.CreateClient());

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
		// The entry point exited without ever building an IHost.
		_output.WriteLine(exception.Message);
	}
}
