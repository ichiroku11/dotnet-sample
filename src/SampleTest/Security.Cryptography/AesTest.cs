using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Security.Cryptography;

public class AesTest {
	// 暗号化
	private static byte[] Encrypt(Aes aes, string plain) {
		using var memoryStream = new MemoryStream();
		using var cryptoStream = new CryptoStream(
			stream: memoryStream,
			transform: aes.CreateEncryptor(),
			mode: CryptoStreamMode.Write,
			leaveOpen: false);

		using var writer = new StreamWriter(cryptoStream);
		writer.Write(plain);
		writer.Close();

		return memoryStream.ToArray();
	}

	// 復号
	private static string Decrypt(Aes aes, byte[] cipher) {
		using var memoryStream = new MemoryStream(cipher);
		using var cryptoStream = new CryptoStream(
			stream: memoryStream,
			transform: aes.CreateDecryptor(),
			mode: CryptoStreamMode.Read,
			leaveOpen: false);

		using var reader = new StreamReader(cryptoStream);
		return reader.ReadToEnd();
	}

	private static string GetHexString(byte[] bytes) {
		return BitConverter.ToString(bytes).ToLower().Replace("-", "");
	}

	private readonly ITestOutputHelper _output;

	public AesTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void 暗号化と復号を試す() {
		// Arrange
		using var aes = Aes.Create();

		// Act
		// 暗号化
		var cipher = Encrypt(aes, "あいうえお");
		_output.WriteLine(GetHexString(cipher));

		// 復号
		var actual = Decrypt(aes, cipher);

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
		var cipher = Encrypt(aes1, "あいうえお");
		_output.WriteLine(GetHexString(cipher));
		var actual = Decrypt(aes2, cipher);

		// Assert
		Assert.Equal("あいうえお", actual);
	}
}
