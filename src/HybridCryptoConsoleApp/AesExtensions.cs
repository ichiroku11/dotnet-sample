using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HybridCryptoConsoleApp {
	public static class AesExtensions {
		// AESで暗号化する
		public static byte[] Encrypt(this Aes aes, string plain) {
			using var memoryStream = new MemoryStream();
			using var cryptoStream = new CryptoStream(
				stream: memoryStream,
				transform: aes.CreateEncryptor(),
				mode: CryptoStreamMode.Write,
				leaveOpen: false);

			using var writer = new StreamWriter(cryptoStream);
			writer.Write(plain);
			// MemoryStream.ToArrayするためにクローズする
			writer.Close();

			return memoryStream.ToArray();
		}

		// AESで復号する
		public static string Decrypt(this Aes aes, byte[] cipher) {
			using var memoryStream = new MemoryStream(cipher);
			using var cryptoStream = new CryptoStream(
				stream: memoryStream,
				transform: aes.CreateDecryptor(),
				mode: CryptoStreamMode.Read,
				leaveOpen: false);

			using var reader = new StreamReader(cryptoStream);
			return reader.ReadToEnd();
		}
	}
}
