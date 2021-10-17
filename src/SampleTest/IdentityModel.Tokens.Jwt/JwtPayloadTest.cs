using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
		public void Constructor_クレームからペイロードを生成する() {
			// Arrange
			var claims = new[] {
				new Claim(JwtRegisteredClaimNames.Iss, "i"),
				new Claim(JwtRegisteredClaimNames.Aud, "a"),
			};

			// Act
			var payload = new JwtPayload(claims);

			// Assert
			Assert.Null(payload.Sub);
			Assert.Null(payload.Nonce);
			Assert.Null(payload.Nbf);
			Assert.Equal("i", payload.Iss);
			Assert.Null(payload.Iat);
			Assert.Null(payload.Exp);
			var audience = Assert.Single(payload.Aud);
			Assert.Equal("a", audience);

			_output.WriteLine(payload.SerializeToJson());
		}

		[Fact]
		public void Constructor_issuerとaudienceからペイロードを生成する() {
			// Arrange
			// Act
			var payload = new JwtPayload(issuer: "i", audience: "a", claims: null, notBefore: null, expires: null);

			// Assert
			Assert.Null(payload.Sub);
			Assert.Null(payload.Nonce);
			Assert.Null(payload.Nbf);
			Assert.Equal("i", payload.Iss);
			Assert.Null(payload.Iat);
			Assert.Null(payload.Exp);
			var audience = Assert.Single(payload.Aud);
			Assert.Equal("a", audience);

			_output.WriteLine(payload.SerializeToJson());
		}

		[Fact]
		public void Constructor_issuerとaudienceとnotBeforeとexpiresを指定してペイロードを生成する() {
			// Arrange
			// Act
			var payload = new JwtPayload(
				issuer: "i",
				audience: "a",
				claims: null,
				notBefore: EpochTime.UnixEpoch.AddSeconds(1),
				expires: EpochTime.UnixEpoch.AddSeconds(2));

			// Assert
			Assert.Null(payload.Sub);
			Assert.Null(payload.Nonce);
			Assert.Equal(1, payload.Nbf);
			Assert.Equal("i", payload.Iss);
			Assert.Null(payload.Iat);
			Assert.Equal(2, payload.Exp);
			var audience = Assert.Single(payload.Aud);
			Assert.Equal("a", audience);

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
