using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace SampleTest.IdentityModel.Protocols.OpenIdConnect;

public class OpenIdConnectProtocolValidatorTest {
	[Fact]
	public void Properties_staticプロパティの値を確認する() {
		// Arrange
		// Act
		// Assert
		Assert.Equal(TimeSpan.FromMinutes(60), OpenIdConnectProtocolValidator.DefaultNonceLifetime);
		Assert.True(OpenIdConnectProtocolValidator.RequireSubByDefault);
	}

	[Fact]
	public void Properties_デフォルトコンストラクターで生成したインスタンスの各プロパティの値を確認する() {
		// Arrange
		var validator = new OpenIdConnectProtocolValidator();

		// Act
		// Assert
		Assert.NotNull(validator.CryptoProviderFactory);
		Assert.NotEmpty(validator.HashAlgorithmMap);
		Assert.Null(validator.IdTokenValidator);
		Assert.Equal(TimeSpan.FromMinutes(60), validator.NonceLifetime);
		Assert.False(validator.RequireAcr);
		Assert.False(validator.RequireAmr);
		Assert.False(validator.RequireAuthTime);
		Assert.False(validator.RequireAzp);
		Assert.True(validator.RequireNonce);
		Assert.True(validator.RequireState);
		Assert.True(validator.RequireStateValidation);
		Assert.True(validator.RequireSub);
		Assert.True(validator.RequireTimeStampInNonce);
	}

	// todo:
	// ValidateAuthenticationResponse
	// ValidateTokenResponse
	// ValidateUserInfoResponse
}
