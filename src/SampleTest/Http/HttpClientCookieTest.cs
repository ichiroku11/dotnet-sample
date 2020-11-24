using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Http {
	public class HttpClientCookieTest : IDisposable {
		private class Startup {
			public void ConfigureServices(IServiceCollection services) {
			}

			private static void ConfigureCookieTest(IApplicationBuilder app) {
				app.Run(async context => {
					if (context.Request.Cookies.Count <= 0) {
						context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
						return;
					}

					// Cookieヘッダをレスポンスとして出力
					var cookies = context.Request.Cookies.Select(cookie => $"{cookie.Key}={cookie.Value}");
					await context.Response.WriteAsync(string.Join('$', cookies));
				});
			}

			private static void ConfigureSetCookieTest(IApplicationBuilder app) {
				app.Run(context => {
					// Set-Cookieヘッダを付与
					context.Response.Cookies.Append("abc", "xyz");
					return Task.CompletedTask;
				});
			}

			public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
				app.Map("/cookie", ConfigureCookieTest);
				app.Map("/setcookie", ConfigureSetCookieTest);

				app.Run(context => {
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					return Task.CompletedTask;
				});
			}
		}

		private readonly TestServer _server;

		public HttpClientCookieTest() {
			_server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
		}

		public void Dispose() {
			_server.Dispose();
		}

		[Fact]
		public async Task レスポンスのSetCookieヘッダを取得する() {
			// Arrange
			using var client = _server.CreateClient();

			// Act
			var response = await client.GetAsync("/setcookie");
			var statusCode = response.StatusCode;
			var containsSetCookie = response.Headers.TryGetValues(HeaderNames.SetCookie, out var cookieValues);

			// Assert
			Assert.Equal(HttpStatusCode.OK, statusCode);
			Assert.True(containsSetCookie);
			Assert.Equal("abc=xyz; path=/", cookieValues.Single());
		}

		[Fact]
		public async Task Cookieヘッダを送信しない_デフォルトの動き() {
			// Arrange
			using var client = _server.CreateClient();

			// Act
			var response = await client.GetAsync("/cookie");
			var statusCode = response.StatusCode;

			// Assert
			Assert.Equal(HttpStatusCode.BadRequest, statusCode);
		}

		[Fact]
		public async Task Cookieヘッダを送信する() {
			// Arrange
			using var client = _server.CreateClient();
			client.DefaultRequestHeaders.Add(HeaderNames.Cookie, "abc=xyz");

			// Act
			var response = await client.GetAsync("/cookie");
			var statusCode = response.StatusCode;
			var responseContent = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal(HttpStatusCode.OK, statusCode);
			Assert.Equal("abc=xyz", responseContent);
		}

		[Fact]
		public async Task CookieContainerを使ってCookieをやり取りする() {
			// Arrange
			using var handler = new CookieContainerHandler {
				InnerHandler = _server.CreateHandler(),
			};
			using var client = new HttpClient(handler) {
				BaseAddress = _server.BaseAddress,
			};
			Assert.Equal(0, handler.Container.Count);
			Assert.Empty(handler.Container.GetCookies(_server.BaseAddress));

			// Act
			// レスポンスにSet-Cookieヘッダが含まれる
			var response1 = await client.GetAsync("/setcookie");
			var statusCode1 = response1.StatusCode;

			var response2 = await client.GetAsync("/cookie");
			var statusCode2 = response2.StatusCode;
			var response2Content = await response2.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal(1, handler.Container.Count);
			Assert.Single(handler.Container.GetCookies(_server.BaseAddress));
			Assert.Equal(HttpStatusCode.OK, statusCode1);
			Assert.Equal(HttpStatusCode.OK, statusCode2);
			Assert.Equal("abc=xyz", response2Content);
		}
	}
}
