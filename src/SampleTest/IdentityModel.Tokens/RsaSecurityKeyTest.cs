using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace SampleTest.IdentityModel.Tokens;

public class RsaSecurityKeyTest {
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void Rsa_RSAParametersでインスタンスを生成するとプロパティはnullになる(bool includePrivateParameters) {
		// Arrange
		using var rsa = RSA.Create();
		var parameters = rsa.ExportParameters(includePrivateParameters);
		var key = new RsaSecurityKey(parameters);

		// Act
		// Assert
		Assert.Null(key.Rsa);
		// 念のためRSAパラメーターが同じことを確認
		Assert.StrictEqual(parameters, key.Parameters);
	}

	[Fact]
	public void Prameters_RSAでインスタンスを生成するとプロパティはデフォルト値になる() {
		// Arrange
		using var rsa = RSA.Create();
		var key = new RsaSecurityKey(rsa);

		// Act
		// Assert
		Assert.StrictEqual(default, key.Parameters);
	}

	[Theory]
	[InlineData(true, PrivateKeyStatus.Exists)]
	[InlineData(false, PrivateKeyStatus.DoesNotExist)]

	public void PrivateKeyStatus_秘密鍵の有無を確認する(bool includePrivateParameters, PrivateKeyStatus expected) {
		// Arrange
		using var rsa = RSA.Create();
		var key = new RsaSecurityKey(rsa.ExportParameters(includePrivateParameters));

		// Act
		// Assert
		Assert.StrictEqual(expected, key.PrivateKeyStatus);
	}

	[Fact]
	public void KeyId_指定しない場合デフォルトではnull() {
		// Arrange
		using var rsa = RSA.Create();
		var key = new RsaSecurityKey(rsa);

		// Act
		// Assert
		Assert.Null(key.KeyId);
	}
}
