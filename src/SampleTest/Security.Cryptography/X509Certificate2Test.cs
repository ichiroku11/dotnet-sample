using SampleLib.AspNetCore;
using System.Security.Cryptography.X509Certificates;

namespace SampleTest.Security.Cryptography;

public class X509Certificate2Test {
	[Fact]
	public void Export_エクスポートしたバイト配列を元にインスタンスを生成するとPrivateKeyをクリアできる() {
		// Arrange
		using var certificate1 = X509Certificate2Helper.GetDevelopmentCertificate();

		if (certificate1 is null) {
			Assert.Fail();
			return;
		}

		// Act
		using var certificate2 = new X509Certificate2(certificate1.Export(X509ContentType.Cert));

		// Assert
		Assert.True(certificate1.HasPrivateKey);
		Assert.False(certificate2.HasPrivateKey);
	}
}
