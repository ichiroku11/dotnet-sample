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
	public class JwtPayloadTest {
		private readonly ITestOutputHelper _output;

		public JwtPayloadTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void Constructor_空のペイロードを生成する() {
			// Arrange
			var payload = new JwtPayload {
			};

			// Act
			// Assert
			Assert.Null(payload.Sub);
			Assert.Null(payload.Nonce);
			Assert.Null(payload.Nbf);
			Assert.Null(payload.Iss);
			Assert.Null(payload.Iat);
			Assert.Null(payload.Exp);
			Assert.Empty(payload.Aud);

			_output.WriteLine(payload.SerializeToJson());
		}


		[Fact]
		public void SerializeToJson_空のペイロードをJSONにシリアライズする() {
			// Arrange
			var payload = new JwtPayload {
			};

			// Act
			var actual = payload.SerializeToJson();

			// Assert
			Assert.Equal("{}", actual);
		}

		[Fact]
		public void Base64UrlEncode_空のペイロードをBase64Urlにエンコードする() {
			// Arrange
			var payload = new JwtPayload {
			};

			// Act
			var actual = payload.Base64UrlEncode();
			_output.WriteLine(actual);

			// Assert
			Assert.Equal(Base64UrlEncoder.Encode("{}"), actual);
		}

	}
}
