using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SampleTest.AspNetCore.Mvc.Testing;

// https://learn.microsoft.com/ja-jp/aspnet/core/test/integration-tests?view=aspnetcore-8.0
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
		// コンパイルエラー
		/*
		public static void Main(string[] args) {
			CreateWebHostBuilder(args).Build().Run();
		}
		*/

		// このシグネチャのメソッドが存在すればよいかと思ったが
		// エントリポイントが必要みたい
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
			return new WebHostBuilder();
		}
	}

	[Fact]
	public void CreateClient_TEntryPointにCreateWebHostBuilderメソッドを持つクラスを指定しても例外が発生する() {
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
