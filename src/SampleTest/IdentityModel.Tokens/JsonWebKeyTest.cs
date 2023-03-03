using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace SampleTest.IdentityModel.Tokens;

public class JsonWebKeyTest {
	public static TheoryData<JsonWebKey> GetTheoryDataForKeyIdIsNull() {
		var key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef"));

		using var rsa = RSA.Create();
		var key2 = new RsaSecurityKey(rsa.ExportParameters(false));

		using var ecdsa = ECDsa.Create();
		var key3 = new ECDsaSecurityKey(ecdsa);

		return new() {
			{ new JsonWebKey() },
			{ JsonWebKeyConverter.ConvertFromSymmetricSecurityKey(key1) },
			{ JsonWebKeyConverter.ConvertFromRSASecurityKey(key2) },
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
			{ JsonWebKeyConverter.ConvertFromSymmetricSecurityKey(key1), "oct" },
			{ JsonWebKeyConverter.ConvertFromRSASecurityKey(key2), "RSA" },
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
}
