using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace SampleTest.AspNetCore.TestHost;

public class TestServerTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class Startup {
		// 定義がなくても良さそう
		/*
		public void ConfigureServices(IServiceCollection services) {
		}
		*/
		public void Configure(IApplicationBuilder app) {
			app.Run(context => {
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				return Task.CompletedTask;
			});
		}
	}
	[Fact]
	public void Properties_インスタンスのプロパティを確認する() {
		// Arrange
		var services = new ServiceCollection().BuildServiceProvider();
		using var server = new TestServer(services);

		// Act
		// Assert
		Assert.False(server.AllowSynchronousIO);
		Assert.NotNull(server.Features);
		Assert.Equal("http://localhost/", server.BaseAddress.AbsoluteUri);
		Assert.False(server.PreserveExecutionContext);
		Assert.Same(services, server.Services);
	}

	[Fact]
	public async Task CreateClient_例外が発生する() {
		// Arrange
		var services = new ServiceCollection().BuildServiceProvider();
		using var server = new TestServer(services);

		// Act
		var exception = Record.Exception(() => {
			_ = server.CreateClient();
		});

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
		_output.WriteLine(exception.Message);
		// The server has not been started or no web application was configured.
	}
}
