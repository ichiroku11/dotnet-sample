using Microsoft.IdentityModel.Tokens;
using SampleLib.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtSecurityTokenHandlerValidateTokenEncryptingTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void CreateJwtSecurityToken_対称鍵で暗号化したトークンを復号する() {
		// Arrange
		// JWTの生成
		string createJwt() {
			var descriptor = new SecurityTokenDescriptor {
				Audience = "audience",
				Issuer = "issuer",
				EncryptingCredentials = new EncryptingCredentials(
					// 暗号化する共通鍵
					new SymmetricSecurityKey(TestSecrets.Default()),
					JwtConstants.DirectKeyUseAlg,
					SecurityAlgorithms.Aes128CbcHmacSha256),
				Claims = new Dictionary<string, object> {
					// 暗号化する情報
					["protected-claim-key"] = "protected-claim-value",
				}
			};

			return new JwtSecurityTokenHandler().CreateJwtSecurityToken(descriptor).RawData;
		}
		var jwt = createJwt();
		Assert.Matches(JwtConstants.JweCompactSerializationRegex, jwt);
		_output.WriteLine(jwt);

		var parameters = new TokenValidationParameters {
			ValidAudience = "audience",
			ValidIssuer = "issuer",
			RequireSignedTokens = false,
			// 復号する共通鍵
			TokenDecryptionKey = new SymmetricSecurityKey(TestSecrets.Default()),
		};

		// Act
		var principal = new JwtSecurityTokenHandler().ValidateToken(jwt, parameters, out var validatedToken);

		// Assert
		Assert.NotNull(principal);
		Assert.Single(principal.Claims,
			claim =>
				string.Equals(claim.Type, "protected-claim-key", StringComparison.OrdinalIgnoreCase) &&
				string.Equals(claim.Value, "protected-claim-value", StringComparison.OrdinalIgnoreCase));
		Assert.IsType<JwtSecurityToken>(validatedToken);
	}

	[Fact]
	public void CreateJwtSecurityToken_非対称鍵で暗号化したトークンを復号する() {
		// Arrange
		// 復号する側の鍵ペア
		using var decryptor = ECDsa.Create();
		// 復号する側の公開鍵
		using var decryptorPublic = ECDsa.Create(decryptor.ExportParameters(false));

		string createJwt() {
			// 暗号化する側の鍵ペア
			using var encryptor = ECDsa.Create();
			// 暗号化する側の公開鍵
			using var encryptorPublic = ECDsa.Create(encryptor.ExportParameters(false));

			var descriptor = new SecurityTokenDescriptor {
				Audience = "audience",
				Issuer = "issuer",
				EncryptingCredentials = new EncryptingCredentials(
					// 暗号化する側の秘密鍵
					new ECDsaSecurityKey(encryptor),
					SecurityAlgorithms.EcdhEsA128kw,
					SecurityAlgorithms.Aes128CbcHmacSha256) {
					// 復号する側の公開鍵
					KeyExchangePublicKey = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(new ECDsaSecurityKey(decryptorPublic)),
				},
				Claims = new Dictionary<string, object> {
					// 暗号化する情報
					["protected-claim-key"] = "protected-claim-value",
				},
				AdditionalHeaderClaims = new Dictionary<string, object> {
					// 暗号化する側の公開鍵
					[JwtHeaderParameterNames.Epk] = JsonSerializer.SerializeToElement(
						JsonWebKeyConverter.ConvertFromECDsaSecurityKey(new ECDsaSecurityKey(encryptorPublic)),
						new JsonSerializerOptions {
							DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
							TypeInfoResolver = new DefaultJsonTypeInfoResolver {
								Modifiers = {
									JsonTypeInfoModifiers.ReturnNullIfCollectionEmpty,
								}
							}
						}),
				},
			};

			return new JwtSecurityTokenHandler().CreateJwtSecurityToken(descriptor).RawData;
		}
		var jwt = createJwt();
		Assert.Matches(JwtConstants.JweCompactSerializationRegex, jwt);
		_output.WriteLine(jwt);

		var parameters = new TokenValidationParameters {
			ValidAudience = "audience",
			ValidIssuer = "issuer",
			RequireSignedTokens = false,
			// 復号する鍵を解決する
			TokenDecryptionKeyResolver = (encodedToken, securityToken, kid, validationParameters) => {
				// 不具合か
				// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1951

				if (securityToken is not JwtSecurityToken token) {
					// ありえるか
					return null;
				}

				// 暗号化する側の公開鍵は"epk"から取り出す
				if (!token.Header.TryGetValue(JwtHeaderParameterNames.Epk, out var epk)) {
					// nullでいいか
					return null;
				}

				if (epk is not JsonElement element) {
					return null;
				}

				var jwk = element.Deserialize<JsonWebKey>();
				if (jwk is null) {
					return null;
				}
				var ecParams = jwk.GetECParameters();

				validationParameters.TokenDecryptionKey = new ECDsaSecurityKey(ECDsa.Create(ecParams));

				// 復号する側の秘密鍵
				return new[] { new ECDsaSecurityKey(decryptor) };
			},
		};

		// Act
		var principal = new JwtSecurityTokenHandler().ValidateToken(jwt, parameters, out var validatedToken);

		// Assert
		Assert.NotNull(principal);
		Assert.Single(principal.Claims,
			claim =>
				string.Equals(claim.Type, "protected-claim-key", StringComparison.OrdinalIgnoreCase) &&
				string.Equals(claim.Value, "protected-claim-value", StringComparison.OrdinalIgnoreCase));
		Assert.IsType<JwtSecurityToken>(validatedToken);
	}
}
