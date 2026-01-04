using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
		Assert.Equal("http://localhost/", server.BaseAddress.AbsoluteUri);
		Assert.NotNull(server.Features);
		Assert.False(server.PreserveExecutionContext);
		Assert.Same(services, server.Services);
	}

	[Fact]
	public void Properties_オプションを使って生成したインスタンスのプロパティを確認する() {
		// Arrange
		var services = new ServiceCollection().BuildServiceProvider();
		var options = new TestServerOptions {
			AllowSynchronousIO = true,
			BaseAddress = new Uri("https://localhost/"),
			PreserveExecutionContext = true,
		};
		using var server = new TestServer(services, Options.Create(options));

		// Act
		// Assert
		Assert.Equal(options.AllowSynchronousIO, server.AllowSynchronousIO);
		Assert.Equal(options.BaseAddress.AbsoluteUri, server.BaseAddress.AbsoluteUri);
		Assert.NotNull(server.Features);
		Assert.Equal(options.PreserveExecutionContext, server.PreserveExecutionContext);
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
