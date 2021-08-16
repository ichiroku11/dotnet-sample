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
		private readonly ITestOutputHelper _output;

		public AesTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void CryptoStream_暗号化と復号を試す() {
			// Arrange
			var aes = Aes.Create();

			// 暗号化
			byte[] encrypt(string plain) {
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
			string decrypt(byte[] cipher) {
				using var memoryStream = new MemoryStream(cipher, false);
				using var cryptoStream = new CryptoStream(
					stream: memoryStream,
					transform: aes.CreateDecryptor(),
					mode: CryptoStreamMode.Read,
					leaveOpen: false);

				using var reader = new StreamReader(cryptoStream);
				return reader.ReadToEnd();
			}

			// Act
			var cipher = encrypt("あいうえお");
			_output.WriteLine(BitConverter.ToString(cipher).ToLower().Replace("-", ""));
			var actual = decrypt(cipher);

			// Assert
			Assert.Equal("あいうえお", actual);
		}
	}
}
