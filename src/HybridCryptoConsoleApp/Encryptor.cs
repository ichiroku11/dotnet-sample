using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HybridCryptoConsoleApp {
	// RSA、AESによる暗号化を行う
	public class Encryptor {
		private readonly RSAParameters _parameters;

		public Encryptor(RSAParameters parameters) {
			_parameters = parameters;
		}

		// 暗号化
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
			// メッセージをAESで暗号化する
			var cipher = aes.Encrypt(plain);
			// IVを暗号化する
			var iv = formatter.CreateKeyExchange(aes.IV);
			// 共通鍵を暗号化してセッションキーとする
			var key = formatter.CreateKeyExchange(aes.Key);

			return (cipher, iv, key);
		}
	}
}
