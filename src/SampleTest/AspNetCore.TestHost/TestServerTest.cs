using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
