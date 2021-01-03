using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SampleLib.AspNetCore.Test {
	public class HttpRequestExtensionsTest {
		private static HttpRequest CreateRequest(string scheme, string host, string pathBase) {
			var context = new DefaultHttpContext();
			var request = context.Request;

			request.Scheme = scheme;
			request.Host = new HostString(host);
			request.PathBase = new PathString(pathBase);

			return request;
		}

		private static HttpRequest CreateRequest(IHeaderDictionary headers) {
			var context = new DefaultHttpContext();

			context.Features.Get<IHttpRequestFeature>().Headers = headers;

			return context.Request;
		}

		private readonly ITestOutputHelper _output;

		public HttpRequestExtensionsTest(ITestOutputHelper output) {
			_output = output;
		}

		[Theory]
		[InlineData("https", "example.jp", null, "https://example.jp")]
		[InlineData("https", "example.jp", "/app", "https://example.jp/app")]
		public void GetAppUrl_取得できる(string scheme, string host, string pathBase, string expected) {
			// Arrange
			var request = CreateRequest(scheme, host, pathBase);

			// Act
			var actual = request.GetAppUrl();
			_output.WriteLine(actual);

			// Assert
			Assert.Equal(expected, actual, StringComparer.OrdinalIgnoreCase);
		}

		public static IEnumerable<object[]> GetTestDataForIsAjax() {
			yield return new object[] {
				new HeaderDictionary {
					{ "X-Requested-With", "XMLHttpRequest" },
				},
				true,
			};
			yield return new object[] {
				new HeaderDictionary(),
				false,
			};
		}

		[Theory]
		[MemberData(nameof(GetTestDataForIsAjax))]
		public void IsAjax_判定できる(IHeaderDictionary headers, bool expected) {
			// Arrange
			var request = CreateRequest(headers);

			// Act
			var actual = request.IsAjax();
			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
