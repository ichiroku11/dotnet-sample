using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace SampleTest.IdentityModel.Tokens;

public class JsonWebKeyTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	public static TheoryData<JsonWebKey> GetTheoryData_KeyIdIsNull() {
		var key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef"));

		using var rsa = RSA.Create();
		var key2 = new RsaSecurityKey(rsa.ExportParameters(false));

		using var ecdsa = ECDsa.Create();
		var key3 = new ECDsaSecurityKey(ecdsa);

		return new() {
			{ new JsonWebKey() },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key1) },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key2) },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key3) },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_KeyIdIsNull))]
	public void KeyId_指定しない場合デフォルトではnull(JsonWebKey key) {
		// Arrange
		// Act
		// Assert
		Assert.Null(key.KeyId);
		Assert.Null(key.Kid);
	}

	[Fact]
	public void Properties_コンストラクターで生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		// Act
		var actual = new JsonWebKey();
		_output.WriteLine(actual.ToJson());

		// Assert
		Assert.Null(actual.Kty);
		Assert.Null(actual.K);
		Assert.Null(actual.D);
		Assert.Null(actual.Crv);
		Assert.Null(actual.X);
		Assert.Null(actual.Y);
		Assert.Null(actual.E);
		Assert.Null(actual.N);
	}

	[Fact]
	public void Properties_SymmetricSecurityKeyから生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef"));

		// Act
		var actual = JsonWebKeyConverter.ConvertFromSecurityKey(key);
		_output.WriteLine(actual.ToJson());

		// Assert
		Assert.Equal("oct", actual.Kty);
		// キー値"k"：not null
		Assert.NotNull(actual.K);
		// 秘密鍵
		Assert.Null(actual.D);
		// EC
		Assert.Null(actual.Crv);
		Assert.Null(actual.X);
		Assert.Null(actual.Y);
		// RSA
		Assert.Null(actual.E);
		Assert.Null(actual.N);
	}

	[Fact]
	public void Properties_RsaSecurityKeyから生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		using var rsa = RSA.Create();
		var key = new RsaSecurityKey(rsa.ExportParameters(false));

		// Act
		var actual = JsonWebKeyConverter.ConvertFromSecurityKey(key);
		_output.WriteLine(actual.ToJson());

		// Assert
		Assert.Equal("RSA", actual.Kty);
		// "k"
		Assert.Null(actual.K);
		// 秘密鍵
		Assert.Null(actual.D);
		// EC
		Assert.Null(actual.Crv);
		Assert.Null(actual.X);
		Assert.Null(actual.Y);
		// RSA：not null
		Assert.NotNull(actual.E);
		Assert.NotNull(actual.N);

	}

	[Fact]
	public void Properties_ECDsaSecurityKeyから生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		using var ecdsa = ECDsa.Create();
		var key = new ECDsaSecurityKey(ecdsa);

		// Act
		var actual = JsonWebKeyConverter.ConvertFromSecurityKey(key);
		_output.WriteLine(actual.ToJson());

		// Assert
		Assert.Equal("EC", actual.Kty);
		// "k"
		Assert.Null(actual.K);
		// ECの秘密鍵のパラメーターも含まれる
		Assert.NotNull(actual.D);
		// E：not null
		Assert.Equal("P-521", actual.Crv);
		Assert.NotNull(actual.X);
		Assert.NotNull(actual.Y);
		// RSA
		Assert.Null(actual.E);
		Assert.Null(actual.N);
	}

	[Fact]
	public void Properties_JSONから生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		using var ecdsa = ECDsa.Create();
		var key = new ECDsaSecurityKey(ecdsa);

		var expected = JsonWebKeyConverter.ConvertFromSecurityKey(key);
		var json = expected.ToJson();
		_output.WriteLine(json);

		// Act
		var actual = new JsonWebKey(json);

		// Assert
		Assert.Equal(expected.Kty, actual.Kty);
		Assert.Equal(expected.Crv, actual.Crv);
		Assert.Equal(expected.D, actual.D);
		Assert.Equal(expected.X, actual.X);
		Assert.Equal(expected.Y, actual.Y);
	}

	public static TheoryData<JsonWebKey, bool> GetTheoryData_CanComputeJwkThumbprint() {
		// 対称鍵
		var key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef"));

		// EC
		using var ecdsa1 = ECDsa.Create();
		var key2 = new ECDsaSecurityKey(ecdsa1);

		// 秘密鍵を削除して鍵を生成
		using var ecdsa2 = ECDsa.Create(ecdsa1.ExportParameters(false));
		var key3 = new ECDsaSecurityKey(ecdsa2);

		// RSA
		using var rsa = RSA.Create();
		var key4 = new RsaSecurityKey(rsa);

		// 秘密鍵を削除して鍵を生成
		var key5 = new RsaSecurityKey(rsa.ExportParameters(false));

		return new() {
			// 対称鍵、非対称鍵はThumbprintを計算できる
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key1), true },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key2), true },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key3), true },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key4), true },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key5), true },
			// 単純に生成したインスタンスはThumbprintを計算できない
			{ new JsonWebKey(), false },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_CanComputeJwkThumbprint))]
	public void CanComputeJwkThumbprint_計算できるかどうかを確認する(JsonWebKey jwk, bool expected) {
		// Arrange

		// Act
		var actual = jwk.CanComputeJwkThumbprint();

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<JsonWebKey, bool> GetTheoryData_HasPrivateKey() {
		using var ecdsa1 = ECDsa.Create();
		var key1 = new ECDsaSecurityKey(ecdsa1);

		// 秘密鍵を削除して鍵を生成
		using var ecdsa2 = ECDsa.Create(ecdsa1.ExportParameters(false));
		var key2 = new ECDsaSecurityKey(ecdsa2);

		return new() {
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key1), true },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key2), false },
			{ new JsonWebKey(), false },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_HasPrivateKey))]
	public void HasPrivateKey_秘密鍵の有無を確認する(JsonWebKey jwk, bool expected) {
		// Arrange

		// Act
		var actual = jwk.HasPrivateKey;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void ComputeJwkThumbprint_秘密鍵の情報はThumbprintに含まれない() {
		// Arrange
		using var ecdsa1 = ECDsa.Create();
		var jwk1 = JsonWebKeyConverter.ConvertFromSecurityKey(new ECDsaSecurityKey(ecdsa1));

		using var ecdsa2 = ECDsa.Create(ecdsa1.ExportParameters(false));
		var jwk2 = JsonWebKeyConverter.ConvertFromSecurityKey(new ECDsaSecurityKey(ecdsa1));

		// Act
		var thumbprint1 = jwk1.ComputeJwkThumbprint();
		var thumbprint2 = jwk2.ComputeJwkThumbprint();

		// Assert
		Assert.True(thumbprint1.SequenceEqual(thumbprint2));
	}
}
