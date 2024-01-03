using System.Security.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace SampleLib.Security.Test;

public class AesExtensionsTest {
	private readonly ITestOutputHelper _output;

	public AesExtensionsTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void 暗号化と復号を試す() {
		// Arrange
		using var aes = Aes.Create();

		// Act
		// 暗号化
		var cipher = aes.Encrypt("あいうえお");
		_output.WriteLine(cipher.ToHexString());

		// 復号
		var actual = aes.Decrypt(cipher);

		// Assert
		Assert.Equal("あいうえお", actual);
	}

	[Fact]
	public void 異なるAesインスタンスで暗号化と復号を試す() {
		// Arrange
		using var aes1 = Aes.Create();
		using var aes2 = Aes.Create();
		// 鍵と初期ベクトルを同じにする
		aes2.Key = aes1.Key;
		aes2.IV = aes1.IV;

		// Act
		var cipher = aes1.Encrypt("あいうえお");
		_output.WriteLine(cipher.ToHexString());
		var actual = aes1.Decrypt(cipher);

		// Assert
		Assert.Equal("あいうえお", actual);
	}
}
