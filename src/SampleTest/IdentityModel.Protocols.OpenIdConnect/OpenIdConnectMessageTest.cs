using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace SampleTest.IdentityModel.Protocols.OpenIdConnect;

public class OpenIdConnectMessageTest {
	[Fact]
	public void Properties_インスタンスの各プロパティの値を確認する() {
		// Arrange
		var message = new OpenIdConnectMessage();

		// Act
		// Assert
		Assert.Empty(message.IssuerAddress);
		Assert.Empty(message.Parameters);
		Assert.Equal(OpenIdConnectRequestType.Authentication, message.RequestType);
	}

	public static TheoryData<OpenIdConnectMessage, string> GetTheoryData_BuildRedirectUrl() {
		return new() {
			// デフォルトコンストラクターでインスタンスを生成した場合は空文字
			{
				new OpenIdConnectMessage(),
				""
			},
			// 
			{
				new OpenIdConnectMessage {
					IssuerAddress = "https://example.jp",
					ClientId = "client-id",
				},
				"https://example.jp?client_id=client-id"
			}
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_BuildRedirectUrl))]
	public void BuildRedirectUrl_生成されるURLを確認する(OpenIdConnectMessage message, string expected) {
		// Arrange
		// Act
		var actual = message.BuildRedirectUrl();

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetParameter_存在しないパラメーター名を指定した場合の戻り値はnull() {
		// Arrange
		var message = new OpenIdConnectMessage {
		};

		// Act
		// Assert
		Assert.Null(message.GetParameter("does-not-exist"));
	}

	[Fact]
	public void GetParameter_指定したパラメーター名で値を取得できる値を確認する() {
		// Arrange
		var message = new OpenIdConnectMessage {
			IssuerAddress = "https://example.jp",
			ClientId = "client-id",
			RedirectUri = "https://localhost/signin-oidc",
			ResponseType = OpenIdConnectResponseType.IdToken,
			Scope = "scope",
			ResponseMode = OpenIdConnectResponseMode.FormPost,
			Nonce = "nonce",
			State = "state"
		};

		// Act
		// Assert
		Assert.Equal("client-id", message.GetParameter(OpenIdConnectParameterNames.ClientId));
		Assert.Equal("https://localhost/signin-oidc", message.GetParameter(OpenIdConnectParameterNames.RedirectUri));
		Assert.Equal(OpenIdConnectResponseType.IdToken, message.GetParameter(OpenIdConnectParameterNames.ResponseType));
		Assert.Equal("scope", message.GetParameter(OpenIdConnectParameterNames.Scope));
		Assert.Equal(OpenIdConnectResponseMode.FormPost, message.GetParameter(OpenIdConnectParameterNames.ResponseMode));
		Assert.Equal("nonce", message.GetParameter(OpenIdConnectParameterNames.Nonce));
		Assert.Equal("state", message.GetParameter(OpenIdConnectParameterNames.State));
	}
}
