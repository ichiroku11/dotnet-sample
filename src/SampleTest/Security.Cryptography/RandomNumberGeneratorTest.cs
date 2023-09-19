
using System.Security.Cryptography;

namespace SampleTest.Security.Cryptography;

// ランダムな値を生成するには
// RNGCryptoServiceProviderの代わりにRandomNumberGeneratorを使う
public class RandomNumberGeneratorTest {
	[Fact]
	public void GetNonZeroBytes_使い方を確認する() {
		// Arrange
		using var generator = RandomNumberGenerator.Create();
		var bytes = new byte[32];

		// Act
		generator.GetNonZeroBytes(bytes);

		// Assert
		Assert.All(bytes, actual => Assert.NotEqual(0, actual));
	}
}
