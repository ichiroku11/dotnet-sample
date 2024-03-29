using Microsoft.IdentityModel.Tokens;
using SampleLib.AspNetCore;

namespace SampleTest.IdentityModel.Tokens;

public class X509SecurityKeyTest {
	[Fact]
	public void Constructor_秘密鍵を保持するインスタンスを生成する() {
		// Arrange
		using var certificate = X509Certificate2Helper.GetDevelopmentCertificate();

		// Act
		var key = new X509SecurityKey(certificate);

		// Assert
		Assert.Equal(PrivateKeyStatus.Exists, key.PrivateKeyStatus);
	}

	[Fact]
	public void Constructor_秘密鍵を保持しないインスタンスを生成する() {
		// Arrange
		using var certificate1 = X509Certificate2Helper.GetDevelopmentCertificate();
		if (certificate1 is null) {
			Assert.Fail();
			return;
		}

		using var certificate2 = certificate1.RemovePrivateKey();

		// Act
		var key = new X509SecurityKey(certificate2);

		// Assert
		Assert.Equal(PrivateKeyStatus.DoesNotExist, key.PrivateKeyStatus);
	}
}
