using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
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

	public static TheoryData<OpenIdConnectProtocolValidationContext> GetTheoryData_ValidateAuthenticationResponse_Throws() {
		return new() {
			// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Protocols.OpenIdConnect/OpenIdConnectProtocolValidator.cs#L208
			// IDX21333: OpenIdConnectProtocolValidationContext.ProtocolMessage is null, there is no OpenIdConnect Response to validate.
			new OpenIdConnectProtocolValidationContext(),

			// IDX21334: Both 'id_token' and 'code' are null in OpenIdConnectProtocolValidationContext.ProtocolMessage received from Authorization Endpoint. Cannot process the message.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage(),
			},
		};
	}

	public static TheoryData<OpenIdConnectProtocolValidationContext> GetTheoryData_ValidateAuthenticationResponse_Throws_IdToken() {
		return new() {
			// IDX21332: OpenIdConnectProtocolValidationContext.ValidatedIdToken is null. There is no 'id_token' to validate against.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
				},
			},

			// IDX21329: RequireState is 'True' but the OpenIdConnectProtocolValidationContext.State is null. State cannot be validated.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
				},
				ValidatedIdToken = new JwtSecurityToken(),
			},

			// IDX21330: RequireState is 'True', the OpenIdConnect Request contained 'state', but the Response does not contain 'state'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					IdToken = "id-token",
				},
				ValidatedIdToken = new JwtSecurityToken(),
				State = "state",
			},
		};
	}

	public static TheoryData<OpenIdConnectProtocolValidationContext> GetTheoryData_ValidateAuthenticationResponse_Throws_Code() {
		return new() {
			// IDX21329: RequireState is 'True' but the OpenIdConnectProtocolValidationContext.State is null. State cannot be validated.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					Code = "code",
				},
			},

			// IDX21330: RequireState is 'True', the OpenIdConnect Request contained 'state', but the Response does not contain 'state'.
			new OpenIdConnectProtocolValidationContext {
				ProtocolMessage = new OpenIdConnectMessage {
					Code = "code",
				},
				State = "state",
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_ValidateAuthenticationResponse_Throws))]
	[MemberData(nameof(GetTheoryData_ValidateAuthenticationResponse_Throws_IdToken))]
	[MemberData(nameof(GetTheoryData_ValidateAuthenticationResponse_Throws_Code))]
	public void ValidateAuthenticationResponse_Throws(OpenIdConnectProtocolValidationContext context) {
		// Arrange
		var validator = new OpenIdConnectProtocolValidator();

		// Act
		var actual = Record.Exception(() => validator.ValidateAuthenticationResponse(context));

		// Assert
		Assert.IsAssignableFrom<OpenIdConnectProtocolException>(actual);
		_output.WriteLine(actual.Message);
	}

	[Fact]
	public void ValidateAuthenticationResponse_DoesNotThrow_Code() {
		// Arrange
		var context = new OpenIdConnectProtocolValidationContext {
			ProtocolMessage = new OpenIdConnectMessage {
				Code = "code",
				State = "state",
			},
			State = "state",
		};
		var validator = new OpenIdConnectProtocolValidator();

		// Act
		var actual = Record.Exception(() => validator.ValidateAuthenticationResponse(context));

		// Assert
		Assert.Null(actual);
	}

	public static TheoryData<JwtPayload> GetTheoryData_ValidateAuthenticationResponse_Throws_IdTokenPayload() {
		var now = DateTime.Now;
		var expires = now.AddMinutes(60);

		return new() {
			// IDX21314: OpenIdConnectProtocol requires the jwt token to have an 'aud' claim. The jwt did not contain an 'aud' claim, jwt: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			new JwtPayload(),

			// IDX21314: OpenIdConnectProtocol requires the jwt token to have an 'exp' claim. The jwt did not contain an 'exp' claim, jwt: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			new JwtPayload(issuer: "i", audience: "a", claims: null, notBefore: null, expires: null, issuedAt: null),

			// IDX21314: OpenIdConnectProtocol requires the jwt token to have an 'iat' claim. The jwt did not contain an 'iat' claim, jwt: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			new JwtPayload(issuer: "i", audience: "a", claims: null, notBefore: null, expires: DateTime.UnixEpoch, issuedAt: null),

			// IDX21314: OpenIdConnectProtocol requires the jwt token to have an 'sub' claim. The jwt did not contain an 'sub' claim, jwt: '[PII of type 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			new JwtPayload(issuer: "i", audience: "a", claims: [], notBefore: now, expires: expires, issuedAt: now),

			// IDX21320: RequireNonce is 'True'. OpenIdConnectProtocolValidationContext.Nonce and OpenIdConnectProtocol.ValidatedIdToken.Nonce are both null or empty. The nonce cannot be validated. If you don't need to check the nonce, set OpenIdConnectProtocolValidator.RequireNonce to 'false'.
			new JwtPayload(issuer: "i", audience: "a", claims: [new(type: "sub", value: "s")], notBefore: now, expires: expires, issuedAt: now),
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_ValidateAuthenticationResponse_Throws_IdTokenPayload))]
	public void ValidateAuthenticationResponse_Throws_IdTokenPayload(JwtPayload payload) {
		// Arrange
		var now = DateTime.Now;
		var context = new OpenIdConnectProtocolValidationContext {
			ProtocolMessage = new OpenIdConnectMessage {
				IdToken = "id-token",
				State = "state",
			},
			ValidatedIdToken = new JwtSecurityToken(header: new JwtHeader(), payload: payload),
			State = "state",
		};
		var validator = new OpenIdConnectProtocolValidator();

		// Act
		var actual = Record.Exception(() => validator.ValidateAuthenticationResponse(context));

		// Assert
		Assert.IsAssignableFrom<OpenIdConnectProtocolException>(actual);
		_output.WriteLine(actual.Message);
	}

	// nonce関連
	public static TheoryData<string> GetTheoryData_ValidateAuthenticationResponse_Throws_IdTokenPayloadNonce() {
		return new() {
			// IDX21325: The 'nonce' did not contain a timestamp: '[PII of type 'System.String' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			// Format expected is: <epochtime>.<noncedata>.
			"nonce",

			// IDX21326: The 'nonce' timestamp could not be converted to a positive integer (greater than 0).
			// timestamp: '0'
			// nonce: '[PII of type 'System.String' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.
			"0.",

			// IDX21324: The 'nonce' has expired: '[PII of type 'System.String' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'. Time from 'nonce' (UTC): '01/01/0001 00:00:00', Current Time (UTC): '04/01/2024 21:08:49'. NonceLifetime is: '01:00:00'.
			"1.",

			// エポックタイムではない？
			// IDX21324: The 'nonce' has expired: '[PII of type 'System.String' is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'. Time from 'nonce' (UTC): '01/01/0001 00:02:51', Current Time (UTC): '04/04/2024 23:01:07'. NonceLifetime is: '01:00:00'.
			$"{EpochTime.GetIntDate(DateTime.UtcNow.AddMinutes(60))}.",
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_ValidateAuthenticationResponse_Throws_IdTokenPayloadNonce))]
	public void ValidateAuthenticationResponse_Throws_IdTokenPayloadNonce(string nonce) {
		// Arrange
		var now = DateTime.Now;
		var context = new OpenIdConnectProtocolValidationContext {
			ProtocolMessage = new OpenIdConnectMessage {
				IdToken = "id-token",
				State = "state",
			},
			ValidatedIdToken = new JwtSecurityToken(
				header: new JwtHeader(),
				payload: new JwtPayload(
					issuer: "i",
					audience: "a",
					claims: [new Claim(type: "sub", value: "s"), new Claim(type: "nonce", value: nonce)],
					notBefore: now,
					expires: now.AddMinutes(60),
					issuedAt: now)),
			State = "state",
			Nonce = nonce,
		};
		var validator = new OpenIdConnectProtocolValidator();

		// Act
		var actual = Record.Exception(() => validator.ValidateAuthenticationResponse(context));

		// Assert
		Assert.IsAssignableFrom<OpenIdConnectProtocolException>(actual);
		_output.WriteLine(actual.Message);
	}

	[Fact]
	public void ValidateAuthenticationResponse_DoesNotThrow_IdToken() {
		// Arrange
		var now = DateTime.Now;
		// バリデーションの中でFromBinaryしてるような？
		var nonce = $"{DateTime.UtcNow.AddMinutes(60).ToBinary()}.";
		var context = new OpenIdConnectProtocolValidationContext {
			ProtocolMessage = new OpenIdConnectMessage {
				IdToken = "id-token",
				State = "state",
			},
			ValidatedIdToken = new JwtSecurityToken(
				header: new JwtHeader(),
				payload: new JwtPayload(
					issuer: "i",
					audience: "a",
					claims: [new Claim(type: "sub", value: "s"), new Claim(type: "nonce", value: nonce)],
					notBefore: now,
					expires: now.AddMinutes(60),
					issuedAt: now)),
			State = "state",
			Nonce = nonce,
		};
		var validator = new OpenIdConnectProtocolValidator();

		// Act
		var actual = Record.Exception(() => validator.ValidateAuthenticationResponse(context));

		// Assert
		Assert.Null(actual);
	}

	// todo:
	// ValidateTokenResponse
	// ValidateUserInfoResponse
}
