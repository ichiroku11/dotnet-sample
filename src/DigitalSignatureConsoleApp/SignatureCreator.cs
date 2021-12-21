using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignatureConsoleApp;

// 署名を生成する
public class SignatureCreator {
	private readonly MessageHasher _hasher;
	private readonly RSAParameters _parameters;

	public SignatureCreator(RSAParameters parameters) {
		_hasher = new MessageHasher();
		_parameters = parameters;
	}

	// メッセージの署名を生成する（メッセージに署名する）
	public byte[] CreateSignature(string message) {
		var messageHash = _hasher.Hash(message);

		using var rsa = RSA.Create(_parameters);
		var formatter = new RSAPKCS1SignatureFormatter(rsa);
		formatter.SetHashAlgorithm(HashAlgorithmName.SHA256.Name!);

		return formatter.CreateSignature(messageHash);
	}
}
