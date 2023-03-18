using System.Security.Cryptography;
using System.Text;

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

	[Theory]
	[InlineData("")]
	[InlineData("あいうえお")]
	[InlineData("0123456789abcdefghijklmnopqrstuvwxyz_0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_0123456789abcdefghijklmnopqrstuvwxyz")]
	public void ComputeHash_ハッシュ値の長さを確認する(string message) {
		// Arrange
		using var hmac = new HMACSHA256();
		var data = Encoding.UTF8.GetBytes(message);

		// Act
		var hash = hmac.ComputeHash(data);

		// Assert
		// 32バイト（256ビット）になる
		Assert.Equal(32, hash.Length);
	}
}
