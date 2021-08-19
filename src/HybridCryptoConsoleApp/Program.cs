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

			// 以下より、IVは暗号化する必要はないが、キーとあわせて暗号化してしまってよいのでは？と解釈
			// https://docs.microsoft.com/ja-jp/dotnet/standard/security/cryptographic-services
			// 共有キー暗号方式の弱点は、両者のキーと IV を一致させ、それぞれの値を転送しておく必要がある点です。
			// IV は秘密情報とは見なされないため、平文のメッセージで転送できます。
			// しかし、キーは承認されていないユーザーから保護する必要があります。
			// このような問題のため、共有キー暗号方式は公開キー暗号方式と併用されることがよくあります。
			// 公開キー暗号方式は、キーと IV の値を秘密に通信するために使用されます。

			using var aes = Aes.Create();
			// IVを暗号化する
			var iv = formatter.CreateKeyExchange(aes.IV);
			// 共通鍵を暗号化してセッションキーとする
			var key = formatter.CreateKeyExchange(aes.Key);

			// メッセージをAESで暗号化
			var cipher = aes.Encrypt(plain);

			return (cipher, iv, key);
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
			// IVとセッションキーを復号して共通鍵を取り出す
			aes.IV = deformatter.DecryptKeyExchange(iv);
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
