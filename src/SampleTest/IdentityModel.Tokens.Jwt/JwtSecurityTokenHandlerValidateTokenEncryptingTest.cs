using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
}
