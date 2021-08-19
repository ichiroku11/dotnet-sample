using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HybridCryptoConsoleApp {
	// RSA、AESによる暗号化を行う
	public class Encryptor {
		private readonly RSAParameters _parameters;

		public Encryptor(RSAParameters parameters) {
			_parameters = parameters;
		}

		public (byte[] cipher, byte[] iv, byte[] key) Encrypt(string plain) {
			using var rsa = RSA.Create(_parameters);
			var formatter = new RSAPKCS1KeyExchangeFormatter(rsa);

			using var aes = Aes.Create();

			// 共通鍵を暗号化してセッションキーとする
			var key = formatter.CreateKeyExchange(aes.Key);

			// メッセージをAESで暗号化
			var cipher = aes.Encrypt(plain);

			return (cipher, aes.IV, key);
		}
	}

	// RSA、AESによる復号を行う
	public class Decryptor {
		private readonly RSAParameters _parameters;

		public Decryptor(RSAParameters parameters) {
			_parameters = parameters;
		}

		public string Decrypt(byte[] cipher, byte[] iv, byte[] key) {
			using var rsa = RSA.Create(_parameters);
			var deformatter = new RSAPKCS1KeyExchangeDeformatter(rsa);

			using var aes = Aes.Create();
			aes.IV = iv;
			// セッションキーを復号して共通鍵を取り出す
			aes.Key = deformatter.DecryptKeyExchange(key);

			// メッセージをAESで復号
			return aes.Decrypt(cipher);
		}
	}

	class Program {
		static void Main(string[] args) {
			// RSAによる鍵交換
			// https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.rsapkcs1keyexchangeformatter?view=net-5.0
			// https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.rsapkcs1keyexchangedeformatter?view=net-5.0
			// AESによる平文の暗号化・復号

			using var rsa = RSA.Create();
			// 公開鍵だけのRSAパラメーター（Encryptorが使用するパラメーター）
			var paramExcludePrivate = rsa.ExportParameters(false);
			// 秘密鍵を含むRSAパラメーター（Decryptorだけが知っているパラメーター）
			var paramIncludePrivate = rsa.ExportParameters(true);

			// 暗号化
			var encryptor = new Encryptor(paramExcludePrivate);
			var (cipher, iv, key) = encryptor.Encrypt("あいうえお");

			// 復号
			var decryptor = new Decryptor(paramIncludePrivate);
			var plain = decryptor.Decrypt(cipher, iv, key);

			Console.WriteLine(plain);
		}
	}
}
