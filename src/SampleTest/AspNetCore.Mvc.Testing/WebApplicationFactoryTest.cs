using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace SampleTest.AspNetCore.Mvc.Testing;

// https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit
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

	private class WebApplicationProgram {
		// このシグネチャのメソッドが存在すればよいかと思ったが
		// エントリポイントが必要みたい
		public static IHostBuilder CreateHostBuilder(string[] args) {
			return new HostBuilder()
				.ConfigureWebHost(builder => {
					builder.UseTestServer();
			});
		}
	}

	[Fact]
	public void CreateClient_TEntryPointにCreateHostBuilderメソッドを持つクラスを指定しても例外が発生する() {
		// Arrange
		using var factory = new WebApplicationFactory<WebApplicationProgram>();

		// Act
		var exception = Record.Exception(() => factory.CreateClient());

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
		// The entry point exited without ever building an IHost.
		_output.WriteLine(exception.Message);
	}
}
