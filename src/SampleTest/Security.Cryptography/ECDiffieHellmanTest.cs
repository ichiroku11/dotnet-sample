
using SampleLib;
using System.Security.Cryptography;

namespace SampleTest.Security.Cryptography;

public class ECDiffieHellmanTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

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
	public void Create_生成したインスタンスごとにパラメーターが異なる() {
		// Arrange
		// Act
		var key1 = ECDiffieHellman.Create().PublicKey.ExportSubjectPublicKeyInfo();
		var key2 = ECDiffieHellman.Create().PublicKey.ExportSubjectPublicKeyInfo();

		_output.WriteLine(Convert.ToHexString(key1));
		_output.WriteLine(Convert.ToHexString(key2));

		// Assert
		Assert.False(key1.SequenceEqual(key2));
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
	public void DeriveKeyFromHash_SHA256を指定するとDeriveKeyMaterialと同じ結果になる様子() {
		// Arrange
		using var alice = ECDiffieHellman.Create();
		using var bob = ECDiffieHellman.Create();

		// Act
		var key1 = alice.DeriveKeyFromHash(bob.PublicKey, HashAlgorithmName.SHA256);
		var key2 = alice.DeriveKeyMaterial(bob.PublicKey);

		// Assert
		Assert.True(key1.SequenceEqual(key2));
	}

	[Fact]
	public void ExportSubjectPublicKeyInfo_公開鍵のエクスポートを試す() {
		// Arrange
		var ecdh = ECDiffieHellman.Create();

		// Act
		// 2つのエクスポートは同じ結果になる様子
		var publicKey1 = ecdh.ExportSubjectPublicKeyInfo();
		var PublicKey2 = ecdh.PublicKey.ExportSubjectPublicKeyInfo();

		// Assert
		Assert.True(publicKey1.SequenceEqual(PublicKey2));
	}

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
