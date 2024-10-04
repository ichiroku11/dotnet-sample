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
	public void Properties_WebHostBuilderを使う場合() {
		// Arrange
		var builder = new WebHostBuilder().UseStartup<Startup>();
		using var server = new TestServer(builder);

		// Act
		// Assert
		Assert.False(server.AllowSynchronousIO);
		Assert.NotNull(server.Features);
		Assert.NotNull(server.Host);
		Assert.Equal("http://localhost/", server.BaseAddress.AbsoluteUri);
		Assert.False(server.PreserveExecutionContext);
		Assert.NotNull(server.Services);
	}

	[Fact]
	public void Properties_WebHostBuilderを使わない場合() {
		// Arrange
		var services = new ServiceCollection().BuildServiceProvider();
		using var server = new TestServer(services);

		// Act
		// Assert
		Assert.False(server.AllowSynchronousIO);
		Assert.NotNull(server.Features);
		// 例外が発生
		//Assert.NotNull(server.Host);
		Assert.Equal("http://localhost/", server.BaseAddress.AbsoluteUri);
		Assert.False(server.PreserveExecutionContext);
		Assert.Same(services, server.Services);
	}

	[Fact]
	public void Host_WebHostBuilderを使わない場合は例外が発生する() {
		// Arrange
		var services = new ServiceCollection().BuildServiceProvider();
		using var server = new TestServer(services);

		// Act
		var exception = Record.Exception(() => server.Host);

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
	}

	[Fact]
	public async Task WebHostBuilderを使ってインスタンスを生成して使ってみる() {
		// Arrange
		using var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
		using var client = server.CreateClient();

		// Act
		var response = await client.GetAsync("/");

		// Assert
		Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
	}
}
