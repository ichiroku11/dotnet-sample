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
}
