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
}
