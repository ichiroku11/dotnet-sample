using System;
using System.Security.Cryptography;
using System.Text;

namespace HybridCryptoConsoleApp {

	public static class AesExtensions {
		public static byte[] Encrypt(this Aes aes, string plain) {
			throw new NotImplementedException();
		}

		public static string Decrypt(this Aes aes, byte[] cipher) {
			throw new NotImplementedException();
		}
	}

	// RSA、AESによる暗号化を行う
	public class Encryptor {

		public byte[] Encrypt(string plain) {
			// todo:
			throw new NotImplementedException();
		}
	}


	// RSA、AESによる復号を行う
	public class Decryptor {
		public string Decrypt(byte[] cipher) {
			// todo:
			throw new NotImplementedException();
		}
	}


	class Program {
		static void Main(string[] args) {

			using var rsa = RSA.Create();

			// 秘密鍵を含むRSAパラメーター（Encryptorだけが知っているパラメーター）
			var paramIncludePrivate = rsa.ExportParameters(true);

			// 公開鍵だけのRSAパラメーター（Decryptorが使用するパラメーター）
			var paramExcludePrivate = rsa.ExportParameters(false);


			// todo:
			// 鍵交換
			// とりあえずRSAによる鍵交換
			// https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.rsapkcs1keyexchangeformatter?view=net-5.0
			// https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.rsapkcs1keyexchangedeformatter?view=net-5.0
			// 平文の暗号化・復号
		}
	}
}
