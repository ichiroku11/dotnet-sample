using System.Security.Cryptography;

namespace SampleTest.Security.Cryptography;

public class HMACSHA256Test {
	[Fact]
	public void Properties_各プロパティを確認する() {
		// Arrange
		using var hmac = new HMACSHA256();

		// Act
		// Assert
		// コンストラクタ引数を省略しても鍵は生成される
		Assert.NotNull(hmac.Key);
		Assert.Equal(64, hmac.Key.Length);
		Assert.Equal(256, hmac.HashSize);
		Assert.Equal(HashAlgorithmName.SHA256.Name, hmac.HashName);
	}
}
