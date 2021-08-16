using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Security.Cryptography {
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

		private readonly ITestOutputHelper _output;

		public AesTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void CryptoStream_暗号化と復号を試す() {
			// Arrange
			var aes = Aes.Create();

			// Act
			var cipher = Encrypt(aes, "あいうえお");
			_output.WriteLine(BitConverter.ToString(cipher).ToLower().Replace("-", ""));
			var actual = Decrypt(aes, cipher);

			// Assert
			Assert.Equal("あいうえお", actual);
		}
	}
}
