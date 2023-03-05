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

	public static TheoryData<JsonWebKey, string?> GetTheoryDataForKty() {
		var key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef"));

		using var rsa = RSA.Create();
		var key2 = new RsaSecurityKey(rsa.ExportParameters(false));

		using var ecdsa = ECDsa.Create();
		var key3 = new ECDsaSecurityKey(ecdsa);

		return new() {
			{ new JsonWebKey(), null },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key1), "oct" },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key2), "RSA" },
			{ JsonWebKeyConverter.ConvertFromSecurityKey(key3), "EC" },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForKty))]
	public void Kty_KeyTypeを確認する(JsonWebKey key, string? expected) {
		// Arrange
		// Act
		// Assert
		Assert.Equal(expected, key.Kty);
	}

	[Fact]
	public void Constructor_JSONから生成したインスタンスを確認する() {
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
}
