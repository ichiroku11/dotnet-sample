using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.IdentityModel.Tokens.Jwt {
	public class JwtSecurityTokenHandlerTest {
		[Fact]
		public void CanValidateToken_trueを返す() {
			// Arrange
			var handler = new JwtSecurityTokenHandler();

			// Act
			// Assert
			Assert.True(handler.CanValidateToken);
		}

		[Fact]
		public void CanWriteToken_trueを返す() {
			// Arrange
			var handler = new JwtSecurityTokenHandler();

			// Act
			// Assert
			Assert.True(handler.CanWriteToken);
		}

		[Theory]
		// null、空文字、空白はfalse
		[InlineData(null, false)]
		[InlineData("", false)]
		[InlineData(" ", false)]
		// header.payload.signatureの形式はtrue
		[InlineData("0.0.0", true)]
		[InlineData("a.a.a", true)]
		// 署名はオプション
		[InlineData("a.a.", true)]
		// ドットは2つ必要
		[InlineData("a.a", false)]
		public void CanReadToken_トークンの形式を判定する(string token, bool expected) {
			// Arrange
			var handler = new JwtSecurityTokenHandler();

			// Act
			// JSON compact serialization formatかどうかを判定する
			var actual = handler.CanReadToken(token);

			// Assert
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void CreateJwtSecurityToken_ペイロードが空の署名なしトークンを生成する() {
			// Arrange
			var handler = new JwtSecurityTokenHandler {
				SetDefaultTimesOnTokenCreation = false,
			};

			// Act
			var token = handler.CreateJwtSecurityToken();

			// Assert
			Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{}", token.ToString());
		}

		[Fact]
		public void CreateJwtSecurityToken_ペイロードがissとaudだけの署名なしトークンを生成する() {
			// Arrange
			var handler = new JwtSecurityTokenHandler {
				SetDefaultTimesOnTokenCreation = false,
			};

			// Act
			var token = handler.CreateJwtSecurityToken(issuer: "i", audience: "a");

			// Assert
			Assert.Equal(@"{""alg"":""none"",""typ"":""JWT""}.{""iss"":""i"",""aud"":""a""}", token.ToString());
		}

		/*
		[Fact]
		public void ValidateToken_HS256で署名したトークンを検証する() {
			// Arrange
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x"));
			var algorithm = SecurityAlgorithms.HmacSha256;
			var credentials = new SigningCredentials(key, algorithm);

			var handler = new JwtSecurityTokenHandler();
			var jwt = handler.CreateEncodedJwt(
				issuer: "issuer",
				audience: "audience",
				subject: null,
				notBefore: null,
				expires: null,
				issuedAt: null,
				signingCredentials: credentials);

			// Act

			// Assert
		}
		*/
	}
}
