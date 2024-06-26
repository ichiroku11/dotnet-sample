using Microsoft.IdentityModel.Tokens;
using SampleLib.AspNetCore;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtSecurityTokenHandlerValidateTokenSigningTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class RsaSecurityKeyHelper {
		private readonly RsaSecurityKey _privateKey;
		private readonly RsaSecurityKey _publicKey;
		private readonly JsonWebKey _jwk;

		public RsaSecurityKeyHelper(RSA rsa) {
			// 秘密パラメーターを含む
			_privateKey = new RsaSecurityKey(rsa.ExportParameters(true));

			// 秘密パラメーターを含まない
			_publicKey = new RsaSecurityKey(rsa.ExportParameters(false));

			// JWK Thumbprintを鍵のIDにする
			_jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(_publicKey);
			_jwk.KeyId = Base64UrlEncoder.Encode(_jwk.ComputeJwkThumbprint());
			_jwk.Use = JsonWebKeyUseNames.Sig;

			_privateKey.KeyId = _jwk.KeyId;
			_publicKey.KeyId = _jwk.KeyId;
		}

		public string JwkThumbprint => _jwk.Kid;

		public SigningCredentials GetSigningCredentials() => new(_privateKey, SecurityAlgorithms.RsaSha256);

		private JsonWebKeySet GetValidationJsonWebKeySet() {
			var jwks = new JsonWebKeySet();
			jwks.Keys.Add(_jwk);
			return jwks;
		}

		public string GetValidationJsonWebKeySetAsJson()
			=> JsonSerializer.Serialize(
				GetValidationJsonWebKeySet(),
				new JsonSerializerOptions {
					DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
					TypeInfoResolver = new DefaultJsonTypeInfoResolver {
						Modifiers = {
							JsonTypeInfoModifiers.ReturnNullIfCollectionEmpty,
						}
					}
				});
	}

	public class TestDataForValidateToken : IEnumerable<object[]>, IDisposable {
		private X509Certificate2? _certificate;

		public TestDataForValidateToken() {
			_certificate = X509Certificate2Helper.GetDevelopmentCertificate();
		}

		public void Dispose() {
			_certificate?.Dispose();
			_certificate = null;

			GC.SuppressFinalize(this);
		}

		public IEnumerator<object[]> GetEnumerator() {
			// HS256
			{
				var key = new SymmetricSecurityKey(TestSecrets.Default());
				yield return new object[] {
					new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
					key,
				};
			}

			// RS256
			// X.509証明書で署名
			yield return new object[] {
				new X509SigningCredentials(_certificate),
				new X509SecurityKey(_certificate?.RemovePrivateKey()),
			};

			// RS256
			// RsaSecurityKeyで署名
			// RsaSecurityKeyをJWKとして公開し、JsonWebKeyを使って署名を検証する運用を想定
			{
				using var rsa = RSA.Create();
				var keyHelper = new RsaSecurityKeyHelper(rsa);

				// 署名のクレデンシャル
				var signingCredentials = keyHelper.GetSigningCredentials();

				// 署名を検証する鍵
				var jwksAsJson = keyHelper.GetValidationJsonWebKeySetAsJson();
				var jwks = new JsonWebKeySet(jwksAsJson);

				yield return new object[] {
					signingCredentials,
					// JsonWebKeyをRsaSecurityKeyに変換しているような気がする
					jwks.GetSigningKeys().First(key => key.KeyId == keyHelper.JwkThumbprint),
				};

				// JsonWebKeyとして検証してみる
				yield return new object[] {
					signingCredentials,
					jwks.Keys.First(key => key.KeyId == keyHelper.JwkThumbprint),
				};
			}
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

		_output.WriteLine(signingCredentials.Key.KeyId ?? "");
		_output.WriteLine(validationKey.KeyId ?? "");

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

		var jwt = token.RawData;
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
		// 署名の鍵
		var key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcd-1"));
		// 検証の鍵
		var key2 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcd-2"));

		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		var token = handler.CreateJwtSecurityToken(
			issuer: "i",
			audience: "a",
			signingCredentials: new SigningCredentials(key1, SecurityAlgorithms.HmacSha256));
		var jwt = token.RawData;

		// Act
		// Assert
		var parameters = new TokenValidationParameters {
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = key2,

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
		var jwt = token.RawData;

		// Act
		// Assert
		var parameters = new TokenValidationParameters {
			// 署名があるものとして検証する
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef")),

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

	[Fact]
	public async Task ValidateTokenAsync_トークンに含まれたオブジェクトの配列を取り出す() {
		// Arrange
		var handler = new JwtSecurityTokenHandler {
			SetDefaultTimesOnTokenCreation = false,
		};

		var descriptor = new SecurityTokenDescriptor {
			// クレームにオブジェクトの配列を追加する
			Claims = new Dictionary<string, object> {
				["test"] = JsonSerializer.SerializeToElement(new[] { new { x = 1 }, new { x = 2 } }),
			}
		};

		var tokenToWrite = handler.CreateJwtSecurityToken(descriptor);
		_output.WriteLine(tokenToWrite.ToString());

		var jwt = tokenToWrite.RawData;
		_output.WriteLine(jwt);

		// Act
		var parameters = new TokenValidationParameters {
			ValidateAudience = false,
			ValidateIssuer = false,
			ValidateIssuerSigningKey = false,
			ValidateLifetime = false,
			// 未署名
			RequireSignedTokens = false,
		};

		// Act
		var result = await handler.ValidateTokenAsync(jwt, parameters);

		// Assert
		{
			var claims = result.ClaimsIdentity.Claims.Where(claim => string.Equals(claim.Type, "test", StringComparison.Ordinal));
			Assert.Equal(2, claims.Count());
			AssertHelper.ContainsClaim(claims, "test", @"{""x"":1}", JsonClaimValueTypes.Json);
			AssertHelper.ContainsClaim(claims, "test", @"{""x"":2}", JsonClaimValueTypes.Json);
		}
		{
			var claims = result.Claims;
			Assert.Single(claims);

			var (key, value) = claims.First();
			Assert.Equal("test", key);

			// valueはIList<object>
			Assert.IsAssignableFrom<IList<object>>(value);
			_output.WriteLine(key);
			_output.WriteLine(value.ToString());

			var values = value as IList<object>;
			Assert.NotNull(values);
			Assert.Equal(2, values.Count);
			// 6.x～
			// dynamic型に変換して"x"プロパティの値を確認
			/*
			Assert.Contains(values, value => ((dynamic)value).x == 1);
			Assert.Contains(values, value => ((dynamic)value).x == 2);
			*/
			// 7.x～
			// オブジェクトはJsonElementになった様子
			// JsonElement型に変換して"x"プロパティの値を確認
			Assert.Contains(values, value => ((JsonElement)value).GetProperty("x").GetInt32() == 1);
			Assert.Contains(values, value => ((JsonElement)value).GetProperty("x").GetInt32() == 2);
		}
	}
}
