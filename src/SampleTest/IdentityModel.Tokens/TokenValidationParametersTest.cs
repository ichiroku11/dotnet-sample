using Microsoft.IdentityModel.Tokens;

namespace SampleTest.IdentityModel.Tokens;

public class TokenValidationParametersTest {
	[Fact]
	public void Constructor_デフォルト値を確認する() {
		// Arrange
		// Act
		var parameters = new TokenValidationParameters();

		// Assert
		Assert.True(parameters.RequireExpirationTime);
		Assert.True(parameters.RequireSignedTokens);
		Assert.True(parameters.RequireAudience);
		Assert.False(parameters.SaveSigninToken);
		Assert.True(parameters.TryAllIssuerSigningKeys);
		Assert.False(parameters.ValidateActor);
		Assert.True(parameters.ValidateAudience);
		Assert.True(parameters.ValidateIssuer);
		Assert.False(parameters.ValidateIssuerSigningKey);
		Assert.True(parameters.ValidateLifetime);
		Assert.False(parameters.ValidateTokenReplay);
	}
}
