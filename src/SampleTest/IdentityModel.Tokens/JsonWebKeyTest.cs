using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace SampleTest.IdentityModel.Tokens;

public class JsonWebKeyTest {
	public static TheoryData<JsonWebKey> GetTheoryDataForKeyIdIsNull() {
		var key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789abcdef"));

		using var rsa = RSA.Create();
		var key2 = new RsaSecurityKey(rsa.ExportParameters(false));

		return new() {
			{ new JsonWebKey() },
			{ JsonWebKeyConverter.ConvertFromSymmetricSecurityKey(key1) },
			{ JsonWebKeyConverter.ConvertFromRSASecurityKey(key2) },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForKeyIdIsNull))]
	public void KeyId_指定しない場合デフォルトではnull(JsonWebKey key) {
		// Arrange
		// Act
		// Assert
		Assert.Null(key.KeyId);
	}
}
