using SampleLib.Security;
using System.Security.Cryptography;

namespace HybridCryptoConsoleApp;

// RSA、AESによる復号を行う
public class Decryptor {
	private readonly RSAParameters _parameters;

	public Decryptor(RSAParameters parameters) {
		_parameters = parameters;
	}

	// 復号
	public string Decrypt(byte[] cipher, byte[] iv, byte[] key) {
		using var rsa = RSA.Create(_parameters);
		var deformatter = new RSAPKCS1KeyExchangeDeformatter(rsa);

		using var aes = Aes.Create();
		// IVとセッションキーを復号して共通鍵を取り出す
		aes.IV = deformatter.DecryptKeyExchange(iv);
		aes.Key = deformatter.DecryptKeyExchange(key);

		// メッセージをAESで復号する
		return aes.Decrypt(cipher);
	}
}
