using Microsoft.IdentityModel.Tokens;
using SampleLib.AspNetCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtSecurityTokenHandlerTest {
	private static readonly SymmetricSecurityKey _key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcd-1"));
	private static readonly SymmetricSecurityKey _key2 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcd-2"));

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
	public void CreateJwtSecurityToken_キーが短いとHS256で署名するときに例外が発生する() {
		// Arrange
		// 文字列ベースでもう1文字いる様子
		var secret = Encoding.UTF8.GetBytes("0123456789abcde");
		var key = new SymmetricSecurityKey(secret);
		_output.WriteLine($"secret.Length: {secret.Length}");
		_output.WriteLine($"key.KeySize: {key.KeySize}");

		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		// Act
		// Assert
		var exception = Assert.Throws<ArgumentOutOfRangeException>(() => {
			handler.CreateJwtSecurityToken(issuer: "i", audience: "a", signingCredentials: credentials);
		});
		_output.WriteLine(exception.Message);
	}

	public class TestDataForValidateToken : IEnumerable<object[]>, IDisposable {
		private X509Certificate2 _certificate;

		public TestDataForValidateToken() {
			_certificate = X509Certificate2Helper.GetDevelopmentCertificate();
		}

		public void Dispose() {
			_certificate?.Dispose();
			_certificate = null;
		}

		public IEnumerator<object[]> GetEnumerator() {
			// HS256
			yield return new object[] {
					new SigningCredentials(_key1, SecurityAlgorithms.HmacSha256),
					_key1,
				};

			// RS256
			yield return new object[] {
					new X509SigningCredentials(_certificate),
					new X509SecurityKey(_certificate.RemovePrivateKey()),
				};
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	[Theory]
	[ClassData(typeof(TestDataForValidateToken))]
	public void ValidateToken_署名したトークンを検証する(
		// 署名に利用する資格情報
		SigningCredentials signingCredentials,
		// 検証用のキー
		SecurityKey validationKey) {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		// 署名付きのトークンを生成
		var token = handler.CreateJwtSecurityToken(
			issuer: "i",
			audience: "a",
			signingCredentials: signingCredentials);
		_output.WriteLine(token.ToString());

		var jwt = handler.WriteToken(token);
		_output.WriteLine(jwt);

		// Act
		var parameters = new TokenValidationParameters {
			// 証明したキーで検証する
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = validationKey,

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

	[Fact]
	public void ValidateToken_署名と検証でキーが異なると例外がスローされる() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		var token = handler.CreateJwtSecurityToken(
			issuer: "i",
			audience: "a",
			signingCredentials: new SigningCredentials(_key1, SecurityAlgorithms.HmacSha256));
		var jwt = handler.WriteToken(token);

		// Act
		var parameters = new TokenValidationParameters {
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = _key2,

			ValidateLifetime = false,
			ValidIssuer = "i",
			ValidAudience = "a"
		};

		// トークンの検証に失敗する
		var exception = Assert.Throws<SecurityTokenSignatureKeyNotFoundException>(() => {
			handler.ValidateToken(jwt, parameters, out var _);
		});
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void ValidateToken_署名なしトークンを署名があるものとして検証すると例外がスローされる() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		// 署名なしトークンを生成
		var token = handler.CreateJwtSecurityToken(issuer: "i", audience: "a");
		var jwt = handler.WriteToken(token);

		// Act
		var parameters = new TokenValidationParameters {
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = _key1,

			ValidateLifetime = false,
			ValidIssuer = "i",
			ValidAudience = "a"
		};

		// トークンの検証に失敗する
		var exception = Assert.Throws<SecurityTokenInvalidSignatureException>(() => {
			handler.ValidateToken(jwt, parameters, out var _);
		});
		_output.WriteLine(exception.Message);
	}
}
