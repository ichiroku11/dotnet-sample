using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.IdentityModel.Tokens.Jwt {
	public class JwtHeaderTest {
		private readonly ITestOutputHelper _output;

		public JwtHeaderTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void SerializeToJson_空のヘッダーをJSONにシリアライズする() {
			// Arrange
			var header = new JwtHeader {
			};

			// Act
			var actual = header.SerializeToJson();

			// Assert
			Assert.Equal("{}", actual);
		}

		[Fact]
		public void Base64UrlEncode_空のヘッダーをBase64Urlにエンコードする() {
			// Arrange
			var header = new JwtHeader {
			};

			// Act
			var actual = header.Base64UrlEncode();
			_output.WriteLine(actual);

			// Assert
			Assert.Equal(Base64UrlEncoder.Encode("{}"), actual);
		}
	}
}
