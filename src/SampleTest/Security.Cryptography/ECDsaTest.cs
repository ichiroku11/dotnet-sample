using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace SampleTest.Security.Cryptography;

public class ECDsaTest {
	[Fact]
	public void Create_生成したインスタンスの各プロパティを確認する() {
		// Arrange
		using var ecdsa = ECDsa.Create();

		// Act
		// Assert
		Assert.Null(ecdsa.KeyExchangeAlgorithm);
		Assert.Equal("ECDsa", ecdsa.SignatureAlgorithm);
	}
}
