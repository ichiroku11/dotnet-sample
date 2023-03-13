using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace SampleTest.IdentityModel.Tokens;

public class JsonWebKeyTest {
	private readonly ITestOutputHelper _output;

	public JsonWebKeyTest(ITestOutputHelper output) {
		_output = output;
	}

	public static TheoryData<JsonWebKey> GetTheoryDataForKeyIdIsNull() {
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
	[MemberData(nameof(GetTheoryDataForKeyIdIsNull))]
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

		// Assert
		Assert.Null(actual.Kty);
	}

	[Fact]
	public void Properties_SymmetricSecurityKeyから生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef"));

		// Act
		var actual = JsonWebKeyConverter.ConvertFromSecurityKey(key);

		// Assert
		Assert.Equal("oct", actual.Kty);
	}

	[Fact]
	public void Properties_RsaSecurityKeyから生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		using var rsa = RSA.Create();
		var key = new RsaSecurityKey(rsa.ExportParameters(false));

		// Act
		var actual = JsonWebKeyConverter.ConvertFromSecurityKey(key);

		// Assert
		Assert.Equal("RSA", actual.Kty);
	}

	[Fact]
	public void Properties_ECDsaSecurityKeyから生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		using var ecdsa = ECDsa.Create();
		var key = new ECDsaSecurityKey(ecdsa);

		// Act
		var actual = JsonWebKeyConverter.ConvertFromSecurityKey(key);

		// Assert
		Assert.Equal("EC", actual.Kty);
	}

	[Fact]
	public void Properties_JSONから生成したJsonWebKeyのプロパティを確認する() {
		// Arrange
		using var ecdsa = ECDsa.Create();
		var key = new ECDsaSecurityKey(ecdsa);

		var expected = JsonWebKeyConverter.ConvertFromSecurityKey(key);
		var json = JsonExtensions.SerializeToJson(expected);
		_output.WriteLine(expected.ToString());
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

	public static TheoryData<JsonWebKey, bool> GetTheoryDataForHasPrivateKey() {
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
	[MemberData(nameof(GetTheoryDataForHasPrivateKey))]
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
