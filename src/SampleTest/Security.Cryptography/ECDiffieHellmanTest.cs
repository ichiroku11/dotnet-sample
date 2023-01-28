
using SampleLib;
using System.Security.Cryptography;

namespace SampleTest.Security.Cryptography;

public class ECDiffieHellmanTest {
	private readonly ITestOutputHelper _output;

	public ECDiffieHellmanTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Create_生成したインスタンスの各プロパティを確認する() {
		// Arrange
		// Act
		var ecdh = ECDiffieHellman.Create();

		// Assert
		Assert.Equal("ECDiffieHellman", ecdh.KeyExchangeAlgorithm);
		Assert.Equal(521, ecdh.KeySize);
		Assert.Null(ecdh.SignatureAlgorithm);

		// サポートされているキーサイズ？何に使うんだろう
		var keySizes = ecdh.LegalKeySizes;
		foreach (var keySize in keySizes) {
			_output.WriteLine($"{nameof(keySize.MinSize)}={keySize.MinSize}, {nameof(keySize.MaxSize)}={keySize.MaxSize}, {nameof(keySize.SkipSize)}={keySize.SkipSize}");
		}
	}
}
