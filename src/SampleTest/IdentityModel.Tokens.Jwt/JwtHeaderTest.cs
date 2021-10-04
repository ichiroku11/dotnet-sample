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

		[Fact]
		public void Constructor_空のヘッダーを生成する() {
			// Arrange
			var header = new JwtHeader();

			// Act
			// Assert
			Assert.Null(header.Typ);
			Assert.Null(header.Alg);

			_output.WriteLine(header.SerializeToJson());
		}

		[Fact]
		public void Constructor_署名なしのヘッダーを生成する() {
			// Arrange
			var header = new JwtHeader(signingCredentials: null);

			// Act
			// Assert
			Assert.Equal("JWT", header.Typ);
			Assert.Equal("none", header.Alg);

			_output.WriteLine(header.SerializeToJson());
		}

		public JwtHeaderTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void SerializeToJson_空のヘッダーをJSONにシリアライズする() {
			// Arrange
			var header = new JwtHeader();

			// Act
			var actual = header.SerializeToJson();

			// Assert
			Assert.Equal("{}", actual);
		}

		[Fact]
		public void Base64UrlEncode_空のヘッダーをBase64Urlにエンコードする() {
			// Arrange
			var header = new JwtHeader();

			// Act
			var actual = header.Base64UrlEncode();
			_output.WriteLine(actual);

			// Assert
			Assert.Equal(Base64UrlEncoder.Encode("{}"), actual);
		}
	}
}
