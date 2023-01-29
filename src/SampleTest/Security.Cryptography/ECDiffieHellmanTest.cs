
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
		// 521？
		Assert.Equal(521, ecdh.KeySize);
		Assert.Null(ecdh.SignatureAlgorithm);

		// サポートされているキーサイズ？何に使うんだろう
		var keySizes = ecdh.LegalKeySizes;
		foreach (var keySize in keySizes) {
			_output.WriteLine($"{nameof(keySize.MinSize)}={keySize.MinSize}, {nameof(keySize.MaxSize)}={keySize.MaxSize}, {nameof(keySize.SkipSize)}={keySize.SkipSize}");
		}
	}

	[Fact]
	public void DeriveKeyMaterial_キーマテリアルを導出する() {
		// Arrange
		var alice = ECDiffieHellman.Create();
		var bob = ECDiffieHellman.Create();

		// Act
		// DeriveKeyMaterialの引数には相手の公開鍵を指定する
		// キーマテリアルとは？
		var derivedKey1 = alice.DeriveKeyMaterial(bob.PublicKey);
		var derivedKey2 = bob.DeriveKeyMaterial(alice.PublicKey);
		_output.WriteLine(derivedKey1.ToHexString());

		// Assert
		// キーマテリアルは一致する
		Assert.True(derivedKey1.SequenceEqual(derivedKey2));
	}
}
