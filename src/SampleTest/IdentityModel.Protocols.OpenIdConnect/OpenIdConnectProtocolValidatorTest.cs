using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SampleTest.IdentityModel.Protocols.OpenIdConnect;

public class OpenIdConnectProtocolValidatorTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

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

	public static TheoryData<OpenIdConnectProtocolValidationContext> GetTheoryData_ValidateAuthenticationResponse_ThrowsException() {
		var now = DateTime.Now;

		return new() {
			// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Protocols.OpenIdConnect/OpenIdConnectProtocolValidator.cs#L208
			// IDX21333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate.
			new OpenIdConnectProtocolValidationContext(),
			// IDX21334: Both 'id_token' and 'code' are null in OpenIdConnectProtocolValidationContext.ProtocolMessage received from Authorization Endpoint. Cannot process the message.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage(),
			},
			// IDX21332: OpenIdConnectProtocolValidationContext.ValidatedIdToken is null. There is no 'id_token' to validate against.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code"
				},
			},
			// IDX21329: RequireState is 'True' but the OpenIdConnectProtocolValidationContext.State is null. State cannot be validated.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
				},
				ValidatedIdToken = new JwtSecurityToken(),
			},
			// IDX21330: RequireState is 'True', the OpenIdConnect Request contained 'state', but the Response does not contain 'state'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
				},
				ValidatedIdToken = new JwtSecurityToken(),
				State = "state",
			},
			// IDX21314: OpenIdConnectProtocol requires the jwt token to have an 'aud' claim. The jwt did not contain an 'aud' claim, jwt: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
					State = "state",
				},
				ValidatedIdToken = new JwtSecurityToken(),
				State = "state",
			},
			// IDX21314: OpenIdConnectProtocol requires the jwt token to have an 'exp' claim. The jwt did not contain an 'exp' claim, jwt: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
					State = "state",
				},
				ValidatedIdToken = new JwtSecurityToken(issuer: "i", audience: "a"),
				State = "state",
			},
			// IDX21314: OpenIdConnectProtocol requires the jwt token to have an 'iat' claim. The jwt did not contain an 'iat' claim, jwt: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
					State = "state",
				},
				ValidatedIdToken = new JwtSecurityToken(issuer: "i", audience: "a", expires: DateTime.UnixEpoch),
				State = "state",
			},
			// IDX21314: OpenIdConnectProtocol requires the jwt token to have an 'sub' claim. The jwt did not contain an 'sub' claim, jwt: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
					State = "state",
				},
				ValidatedIdToken = new JwtSecurityToken(
					header: new JwtHeader(),
					payload: new JwtPayload(
						issuer: "i",
						audience: "a",
						claims: [],
						notBefore: now,
						expires: now.AddMinutes(60),
						issuedAt: now)),
				State = "state",
			},
			// IDX21320: RequireNonce is 'True'. OpenIdConnectProtocolValidationContext.Nonce and OpenIdConnectProtocol.ValidatedIdToken.Nonce are both null or empty. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
					State = "state",
				},
				ValidatedIdToken = new JwtSecurityToken(
					header: new JwtHeader(),
					payload: new JwtPayload(
						issuer: "i",
						audience: "a",
						claims: [new Claim(type: "sub", value: "s")],
						notBefore: now,
						expires: now.AddMinutes(60),
						issuedAt: now)),
				State = "state",
			},
			// IDX21320: RequireNonce is 'True'. OpenIdConnectProtocolValidationContext.Nonce and OpenIdConnectProtocol.ValidatedIdToken.Nonce are both null or empty. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
					State = "state",
				},
				ValidatedIdToken = new JwtSecurityToken(
					header: new JwtHeader(),
					payload: new JwtPayload(
						issuer: "i",
						audience: "a",
						claims: [new Claim(type: "sub", value: "s")],
						notBefore: now,
						expires: now.AddMinutes(60),
						issuedAt: now)),
				State = "state",
			},
			// IDX21325: The 'nonce' did not contain a timestamp: '[PII of type 'System.String' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			// Format expected is: <epochtime>.<noncedata>.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
					Code = "code",
					State = "state",
				},
				ValidatedIdToken = new JwtSecurityToken(
					header: new JwtHeader(),
					payload: new JwtPayload(
						issuer: "i",
						audience: "a",
						claims: [new Claim(type: "sub", value: "s"), new Claim(type: "nonce", value: "nonce")],
						notBefore: now,
						expires: now.AddMinutes(60),
						issuedAt: now)),
				State = "state",
				Nonce = "nonce",
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_ValidateAuthenticationResponse_ThrowsException))]
	public void ValidateAuthenticationResponse_ThrowsException(OpenIdConnectProtocolValidationContext context) {
		// Arrange
		var validator = new OpenIdConnectProtocolValidator();

		// Act
		var actual = Record.Exception(() => validator.ValidateAuthenticationResponse(context));

		// Assert
		Assert.NotNull(actual);
		_output.WriteLine(actual.Message);
	}

	// todo:
	// ValidateTokenResponse
	// ValidateUserInfoResponse
}
