using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HybridCryptoConsoleApp;

class Program {
	static void Main(string[] args) {
		// RSAによる鍵交換
		// https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.rsapkcs1keyexchangeformatter?view=net-5.0
		// https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.rsapkcs1keyexchangedeformatter?view=net-5.0
		// AESによる平文の暗号化・復号

		using var rsa = RSA.Create();

		// 公開鍵だけのRSAパラメーター（Encryptorが使用するパラメーター）
		var paramExcludePrivate = rsa.ExportParameters(false);
		// 暗号化
		var encryptor = new Encryptor(paramExcludePrivate);
		var (cipher, iv, key) = encryptor.Encrypt("あいうえお");

		// 秘密鍵を含むRSAパラメーター（Decryptorだけが知っているパラメーター）
		var paramIncludePrivate = rsa.ExportParameters(true);
		// 復号
		var decryptor = new Decryptor(paramIncludePrivate);
		var plain = decryptor.Decrypt(cipher, iv, key);

		// あいうえお
		Console.WriteLine(plain);
	}
}
