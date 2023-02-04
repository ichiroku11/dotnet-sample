
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

	[Fact]
	public void ExportSubjectPublicKeyInfo_公開鍵のエクスポートを試す() {
		// Arrange
		var ecdh = ECDiffieHellman.Create();

		// Act
		var publicKey1 = ecdh.ExportSubjectPublicKeyInfo();
		var PublicKey2 = ecdh.PublicKey.ExportSubjectPublicKeyInfo();

		// Assert
		Assert.True(publicKey1.SequenceEqual(PublicKey2));
	}

	// X.509 SubjectPublicKeyInfoフォーマットでエクスポートした公開鍵を別のインスタンスでインポートする
	// サーバーとクライアントなど別のプログラムで公開鍵をやりとりする方法はこれかな・・・
	[Fact]
	public void ImportSubjectPublicKeyInfo_SubjectPublicKeyInfo形式でエクスポートした公開鍵をインポートする() {
		// Arrange
		var alice = ECDiffieHellman.Create();
		var bob = ECDiffieHellman.Create();

		// 公開鍵のエクスポート
		// PublicKey.ToByteArrayはObsoleteになった様子
		// https://learn.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.ecdiffiehellmanpublickey.tobytearray?view=net-7.0
		// ECDiffieHellmanPublicKey.ToByteArray() and the associated constructor do not have a consistent and interoperable implementation on all platforms.
		// Use ECDiffieHellmanPublicKey.ExportSubjectPublicKeyInfo() instead.
		var expected = alice.ExportSubjectPublicKeyInfo();

		// Act
		// 公開鍵のインポート
		bob.ImportSubjectPublicKeyInfo(expected, out _);
		var actual = bob.ExportSubjectPublicKeyInfo();

		// Assert
		Assert.Equal(expected, actual);
	}
}
