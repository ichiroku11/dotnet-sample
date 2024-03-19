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

	[Fact]
	public void BuildRedirectUrl_インスタンスを生成しただけで呼び出す空文字になる() {
		// Arrange
		var message = new OpenIdConnectMessage();

		// Act
		var url = message.BuildRedirectUrl();

		// Assert
		Assert.Empty(url);
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
