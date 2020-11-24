using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Http {
	public class HttpClientBaseAddressTest {
		private class Startup {
			public void ConfigureServices(IServiceCollection services) {
			}

			private static void ConfigureApp(IApplicationBuilder app) {
				app.Run(async context => {
					context.Response.ContentType = "text/plain";
					await context.Response.WriteAsync($"home|{context.Request.GetDisplayUrl()}");
				});
			}
			private static void ConfigureApiSub(IApplicationBuilder app) {
				app.Run(async context => {
					context.Response.ContentType = "text/plain";
					await context.Response.WriteAsync($"apisub|{context.Request.GetDisplayUrl()}");
				});
			}

			public void Configure(IApplicationBuilder app) {
				app.Map("/home", ConfigureApp);
				app.Map("/api/sub", ConfigureApiSub);

				app.Run(async context => {
					context.Response.ContentType = "text/plain";
					await context.Response.WriteAsync($"root|{context.Request.GetDisplayUrl()}");
				});
			}
		}

		private static TestServer CreateServer(string baseUri)
			=> new TestServer(new WebHostBuilder().UseStartup<Startup>()) {
				BaseAddress = new Uri(baseUri)
			};

		private readonly ITestOutputHelper _output;

		public HttpClientBaseAddressTest(ITestOutputHelper output) {
			_output = output;
		}

		[Theory]
		// ドメイン直下に配置する場合は最後のスラッシュができる
		[InlineData("http://example.jp", "http://example.jp/")]
		[InlineData("http://example.jp/", "http://example.jp/")]
		// パスに配置する場合は最後のスラッシュがそのままになる
		[InlineData("http://example.jp/app", "http://example.jp/app")]
		[InlineData("http://example.jp/app/", "http://example.jp/app/")]
		public void BaseAddress_TestServerとHttpClientのBaseAddressを確認する(
			string serverBaseUri, string expectedClientBaseUri) {
			// Arrange
			// Act
			using var server = CreateServer(serverBaseUri);
			using var client = server.CreateClient();

			// Assert
			Assert.Equal(expectedClientBaseUri, client.BaseAddress.AbsoluteUri);
		}

		// HttpClient.BaseAddressの最後の「/」やリクエストの相対パスの最初の「/」の有無は気にしなくてよく
		// GetAsyncで指定する相対パスのURLにリクエストを送信できる
		[Theory]
		[InlineData("http://example.jp", "", "http://example.jp/", "root|http://example.jp/")]
		[InlineData("http://example.jp", "/", "http://example.jp/", "root|http://example.jp/")]
		[InlineData("http://example.jp", "home", "http://example.jp/home", "home|http://example.jp/home")]
		[InlineData("http://example.jp", "/home", "http://example.jp/home", "home|http://example.jp/home")]
		[InlineData("http://example.jp", "api/sub", "http://example.jp/api/sub", "apisub|http://example.jp/api/sub")]
		[InlineData("http://example.jp", "/api/sub", "http://example.jp/api/sub", "apisub|http://example.jp/api/sub")]
		[InlineData("http://example.jp/", "", "http://example.jp/", "root|http://example.jp/")]
		[InlineData("http://example.jp/", "/", "http://example.jp/", "root|http://example.jp/")]
		[InlineData("http://example.jp/", "home", "http://example.jp/home", "home|http://example.jp/home")]
		[InlineData("http://example.jp/", "/home", "http://example.jp/home", "home|http://example.jp/home")]
		[InlineData("http://example.jp/", "api/sub", "http://example.jp/api/sub", "apisub|http://example.jp/api/sub")]
		[InlineData("http://example.jp/", "/api/sub", "http://example.jp/api/sub", "apisub|http://example.jp/api/sub")]
		public async Task BaseAddress_ドメイン直下に配置したWebアプリに対して相対パスでリクエストする(
			string baseUri, string requestUri, string expectedUri, string expectedContent) {
			// Arrange
			// サーバの配置URLの「/」はどっちでも良さげ
			using var server = CreateServer("http://example.jp");
			//using var server = CreateServer("http://example.jp/");
			using var client = server.CreateClient();
			client.BaseAddress = new Uri(baseUri);

			// Act
			var response = await client.GetAsync(requestUri);
			var actualUri = response.RequestMessage.RequestUri.AbsoluteUri;
			var actualContent = await response.Content.ReadAsStringAsync();

			// Assert
			_output.WriteLine(actualUri);
			Assert.Equal(expectedUri, actualUri);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(expectedContent, actualContent);
		}

		// レスポンスが返ってくることが不思議な感じもする部分もあるが、
		// リクエストされるURLはBassAddress＋GetAsyncの引数のURLになるっぽい
		[Theory]
		[InlineData("http://example.jp/app", "", "http://example.jp/app", "root|http://example.jp/app")]
		// レスポンスが返ってくるのが不思議
		[InlineData("http://example.jp/app", "/", "http://example.jp/", "root|http://example.jp/")]
		// レスポンスが返ってくるのが不思議
		[InlineData("http://example.jp/app", "home", "http://example.jp/home", "home|http://example.jp/home")]
		// レスポンスが返ってくるのが不思議
		[InlineData("http://example.jp/app", "/home", "http://example.jp/home", "home|http://example.jp/home")]
		// レスポンスが返ってくるのが不思議
		[InlineData("http://example.jp/app", "api/sub", "http://example.jp/api/sub", "apisub|http://example.jp/api/sub")]
		// レスポンスが返ってくるのが不思議
		[InlineData("http://example.jp/app", "/api/sub", "http://example.jp/api/sub", "apisub|http://example.jp/api/sub")]
		[InlineData("http://example.jp/app/", "", "http://example.jp/app/", "root|http://example.jp/app/")]
		// レスポンスが返ってくるのが不思議
		[InlineData("http://example.jp/app/", "/", "http://example.jp/", "root|http://example.jp/")]
		[InlineData("http://example.jp/app/", "home", "http://example.jp/app/home", "home|http://example.jp/app/home")]
		// レスポンスが返ってくるのが不思議
		[InlineData("http://example.jp/app/", "/home", "http://example.jp/home", "home|http://example.jp/home")]
		[InlineData("http://example.jp/app/", "api/sub", "http://example.jp/app/api/sub", "apisub|http://example.jp/app/api/sub")]
		// レスポンスが返ってくるのが不思議
		[InlineData("http://example.jp/app/", "/api/sub", "http://example.jp/api/sub", "apisub|http://example.jp/api/sub")]
		public async Task BaseAddress_パス以下に配置したWebアプリに対して相対パスでリクエストする(
			string baseUri, string requestUri, string expectedUri, string expectedContent) {
			// Arrange
			// サーバの配置URLの「/」はどっちでも良さげ
			//using var server = CreateServer("http://example.jp/app");
			using var server = CreateServer("http://example.jp/app/");
			using var client = server.CreateClient();
			client.BaseAddress = new Uri(baseUri);

			// Act
			var response = await client.GetAsync(requestUri);
			var actualUri = response.RequestMessage.RequestUri.AbsoluteUri;
			var actualContent = await response.Content.ReadAsStringAsync();

			// Assert
			_output.WriteLine(actualUri);
			Assert.Equal(expectedUri, actualUri);
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(expectedContent, actualContent);
		}
	}
}
