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
	public class JwtSecurityTokenHandlerTest {
		private readonly ITestOutputHelper _output;

		public JwtSecurityTokenHandlerTest(ITestOutputHelper output) {
			_output = output;
		}

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

		[Fact]
		public void ValidateToken_HS256で署名したトークンを検証する() {
			// Arrange
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdefghijABCDEFGHIJ01"));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var handler = new JwtSecurityTokenHandler {
				SetDefaultTimesOnTokenCreation = false,
			};

			// 署名付きのトークンを生成
			var token = handler.CreateJwtSecurityToken(issuer: "i", audience: "a", signingCredentials: credentials);
			_output.WriteLine(token.ToString());

			var jwt = handler.WriteToken(token);
			_output.WriteLine(jwt);

			// Act
			var parameters = new TokenValidationParameters {
				// 証明したキーで検証する
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = key,

				ValidateLifetime = false,
				ValidIssuer = "i",
				ValidAudience = "a"
			};
			// 署名付きのトークンを検証
			var principal = handler.ValidateToken(jwt, parameters, out var _);

			// Assert
			Assert.Equal(2, principal.Claims.Count());
			Assert.Contains(
				principal.Claims,
				claim =>
					string.Equals(claim.Type, "iss", StringComparison.OrdinalIgnoreCase) &&
					string.Equals(claim.Value, "i", StringComparison.OrdinalIgnoreCase));
			Assert.Contains(
				principal.Claims,
				claim =>
					string.Equals(claim.Type, "aud", StringComparison.OrdinalIgnoreCase) &&
					string.Equals(claim.Value, "a", StringComparison.OrdinalIgnoreCase));
		}
	}
}
