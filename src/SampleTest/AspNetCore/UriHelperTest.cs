using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Xunit;

namespace SampleTest.AspNetCore {
	public class UriHelperTest {
		[Fact]
		public void BuildAbsolute_とりあえず使ってみる() {
			// Arrange
			// Act
			var url = UriHelper.BuildAbsolute(
				"https",
				new HostString("example.jp"),
				pathBase: "/app",
				path: "/default/index");

			// Assert
			Assert.Equal("https://example.jp/app/default/index", url);
		}

		[Fact]
		public void BuildRelative_とりあえず使ってみる() {
			// Arrange
			// Act
			var url = UriHelper.BuildRelative(
				pathBase: "/app",
				path: "/default/index");

			// Assert
			Assert.Equal("/app/default/index", url);
		}
	}
}
