using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace SampleTest.IdentityModel.Tokens.Jwt;

public class JwtSecurityTokenHandlerValidateTokenEncryptingTest {
	private readonly ITestOutputHelper _output;

	public JwtSecurityTokenHandlerValidateTokenEncryptingTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void CreateJwtSecurityToken_対称鍵で暗号化したトークンを復号する() {
		// Arrange
		var secret = Encoding.UTF8.GetBytes("0123456789abcdef0123456789abcdef");

		// JWTの生成
		string createJwt() {
			var descriptor = new SecurityTokenDescriptor {
				Audience = "audience",
				Issuer = "issuer",
				EncryptingCredentials = new EncryptingCredentials(
					// 暗号化する共通鍵
					new SymmetricSecurityKey(secret),
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
			TokenDecryptionKey = new SymmetricSecurityKey(secret),
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
					[JwtHeaderParameterNames.Epk] = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(new ECDsaSecurityKey(encryptorPublic)),
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
				// 不具合か？
				// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1951

				// todo: 暗号化する側の公開鍵は"epk"から取り出す必要がありそう

				return null;
			},
		};

		// Act
		// todo:
		//var principal = new JwtSecurityTokenHandler().ValidateToken(jwt, parameters, out var validatedToken);

		// Assert
		// todo:
		var exception = Assert.ThrowsAny<SecurityTokenException>(() => {
			new JwtSecurityTokenHandler().ValidateToken(jwt, parameters, out var validatedToken);
		});
		_output.WriteLine(exception.Message);

		// todo:
		/*
		Assert.NotNull(principal);
		Assert.Single(principal.Claims,
			claim =>
				string.Equals(claim.Type, "protected-claim-key", StringComparison.OrdinalIgnoreCase) &&
				string.Equals(claim.Value, "protected-claim-value", StringComparison.OrdinalIgnoreCase));
		Assert.IsType<JwtSecurityToken>(validatedToken);
		*/
	}
}
