using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace SampleTest.AspNetCore {
	public class PathStringTest {
		[Fact]
		public void PathString_とりあえず使ってみる() {
			// Arrange
			// Act
			var path = new PathString("/app");

			// Assert
			Assert.Equal("/app", path.Value);
		}

		[Fact(DisplayName = "PathString_コンストラクタ引数は'/'で始まらないとArgumentExceptionがスローされる")]
		public void PathString_ConstructorArgumentStartWithSlash() {
			// Arrange
			// Act
			// Assert
			Assert.Throws<ArgumentException>(() => {
				new PathString("app");
			});
		}

		[Theory]
		[InlineData("/", "/", true)]
		[InlineData("/app/abc", "/app", true)]
		[InlineData("/app/abc", "/app/abc", true)]
		[InlineData("/app/abc", "/", false)]
		[InlineData("/app/abc", "/app/", false)]
		[InlineData("/app/abc", "/app/t", false)]
		[InlineData("/app/abc/xyz", "/app", true)]
		[InlineData("/app/abc/xyz", "/app/abc", true)]
		[InlineData("/app/abc/xyz", "/app/abc/xyz", true)]
		[InlineData("/app/abc/xyz", "/app/abc/", false)]
		public void StartsWithSegments_インスタンスの先頭が指定されたPathStringと一致するかどうか(string src, string test, bool expected) {
			// Arrange
			// Act
			var actual = new PathString(src).StartsWithSegments(new PathString(test));

			// Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("/app/test", "/app", true)]
		[InlineData("/app/test", "/APP", true)]
		[InlineData("/APP/test", "/app", true)]
		public void StartsWithSegments_大文字小文字を区別しないで比較する(string src, string test, bool expected) {
			// Arrange
			// Act
			var actual = new PathString(src).StartsWithSegments(new PathString(test));

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
