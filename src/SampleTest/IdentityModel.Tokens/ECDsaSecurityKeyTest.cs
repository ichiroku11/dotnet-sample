using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace SampleTest.IdentityModel.Tokens;

public class ECDsaSecurityKeyTest {
	[Fact]
	public void PrivateKeyStatus_引数がECDsaのコンストラクターで生成したインスタンスではUnknownになる() {
		// Arrange
		using var ecdsa = ECDsa.Create();
		var key = new ECDsaSecurityKey(ecdsa);

		// Act
		// Assert
		Assert.Equal(PrivateKeyStatus.Unknown, key.PrivateKeyStatus);
	}
}
