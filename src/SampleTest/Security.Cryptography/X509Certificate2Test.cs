using SampleLib.AspNetCore;
using System.Security.Cryptography.X509Certificates;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Security.Cryptography;

public class X509Certificate2Test {
	private readonly ITestOutputHelper _output;

	public X509Certificate2Test(ITestOutputHelper output) {
		_output = output;
	}

	// PrivateKeyプロパティが非推奨になったためコメントアウト
	/*
	[Fact]
	public void PrivateKey_値を変更しようとすると例外が発生する() {
		// Arrange
		using var certificate = X509Certificate2Helper.GetDevelopmentCertificate();

		// Act
		// Assert
		Assert.True(certificate.HasPrivateKey);
		var exception = Assert.Throws<PlatformNotSupportedException>(() => {
			certificate.PrivateKey = null;
		});
		_output.WriteLine(exception.Message);
	}
	*/

	[Fact]
	public void Export_エクスポートしたバイト配列を元にインスタンスを生成するとPrivateKeyをクリアできる() {
		// Arrange
		using var certificate1 = X509Certificate2Helper.GetDevelopmentCertificate();

		if (certificate1 is null) {
			AssertHelper.Fail();
			return;
		}

		// Act
		using var certificate2 = new X509Certificate2(certificate1.Export(X509ContentType.Cert));

		// Assert
		Assert.True(certificate1.HasPrivateKey);
		Assert.False(certificate2.HasPrivateKey);
	}
}
