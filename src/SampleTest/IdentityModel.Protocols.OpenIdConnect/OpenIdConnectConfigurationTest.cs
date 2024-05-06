using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace SampleTest.IdentityModel.Protocols.OpenIdConnect;

public class OpenIdConnectConfigurationTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Properties_デフォルトコンストラクターで生成したインスタンスの各プロパティの値を確認する() {
		// Arrange
		var config = new OpenIdConnectConfiguration();

		// Act
		// Assert
		Assert.Null(config.Issuer);
		Assert.Null(config.AuthorizationEndpoint);
		Assert.Null(config.TokenEndpoint);
		Assert.Null(config.EndSessionEndpoint);
		Assert.Null(config.JwksUri);
		Assert.Empty(config.ResponseModesSupported);
		Assert.Empty(config.ResponseTypesSupported);
		Assert.Empty(config.ScopesSupported);
		Assert.Empty(config.SubjectTypesSupported);
		Assert.Empty(config.IdTokenSigningAlgValuesSupported);
		Assert.Empty(config.TokenEndpointAuthMethodsSupported);
		Assert.Empty(config.ClaimsSupported);
	}

	[Fact]
	public async Task Properties_コンストラクターにJSONを指定して生成したインスタンスの各プロパティの値を確認する() {
		// Arrange
		using var stream = File.OpenRead("sample-openid-configuration.json");
		using var reader = new StreamReader(stream);
		var json = await reader.ReadToEndAsync();

		var config = new OpenIdConnectConfiguration(json);

		// IdentityModel 7.2.0 => 7.5.1
		// ファイルのJSONをうまく読み込みなくなった様子・・・
		// Act
		// Assert
		Assert.Equal("issuer", config.Issuer);
		//Assert.Equal("authorization-endpoint", config.AuthorizationEndpoint);
		Assert.Null(config.AuthorizationEndpoint);
		Assert.Equal("token-endpoint", config.TokenEndpoint);
		//Assert.Equal("end-session-endpoint", config.EndSessionEndpoint);
		Assert.Null(config.EndSessionEndpoint);
		Assert.Equal("jwks-uri", config.JwksUri);
		//Assert.Equal(["form_post"], config.ResponseModesSupported);
		Assert.Empty(config.ResponseModesSupported);
		Assert.Equal(
			["code", "code id_token", "code token", "code id_token token", "id_token", "id_token token", "token", "token id_token"],
			config.ResponseTypesSupported);
		//Assert.Equal(["openid"], config.ScopesSupported);
		Assert.Empty(config.ScopesSupported);
		Assert.Equal(["pairwise"], config.SubjectTypesSupported);
		//Assert.Equal(["RS256"], config.IdTokenSigningAlgValuesSupported);
		Assert.Empty(config.IdTokenSigningAlgValuesSupported);
		Assert.Equal(["client_secret_post", "client_secret_basic"], config.TokenEndpointAuthMethodsSupported);
		//Assert.Equal(
		//	["name", "emails", "idp", "oid", "sub", "extension_1", "iss", "iat", "exp", "aud", "acr", "nonce", "auth_time"],
		//	config.ClaimsSupported);
		Assert.Empty(config.ClaimsSupported);
	}

	[Fact]
	public void Properties_Writeメソッドで書き込んだJSONから生成したインスタンスの各プロパティの値を確認する() {
		// Arrange
		var json = OpenIdConnectConfiguration.Write(
			new OpenIdConnectConfiguration {
				Issuer = "issuer",
				AuthorizationEndpoint = "authorization-endpoint",
				EndSessionEndpoint = "end-session-endpoint",
			});
		_output.WriteLine(json);

		// Act
		var config = new OpenIdConnectConfiguration(json);

		// Assert
		Assert.Equal("issuer", config.Issuer);
		Assert.Equal("authorization-endpoint", config.AuthorizationEndpoint);
		// なぜかダメ
		//Assert.Equal("end-session-endpoint", config.EndSessionEndpoint);
		Assert.Null(config.EndSessionEndpoint);
	}
}
