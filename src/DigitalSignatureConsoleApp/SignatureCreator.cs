using System.Security.Cryptography;

namespace DigitalSignatureConsoleApp;

// 署名を生成する
public class SignatureCreator(RSAParameters parameters) {
	private readonly MessageHasher _hasher = new MessageHasher();
	private readonly RSAParameters _parameters = parameters;

	// メッセージの署名を生成する（メッセージに署名する）
	public byte[] CreateSignature(string message) {
		var messageHash = _hasher.Hash(message);

		using var rsa = RSA.Create(_parameters);
		var formatter = new RSAPKCS1SignatureFormatter(rsa);
		formatter.SetHashAlgorithm(HashAlgorithmName.SHA256.Name!);

		return formatter.CreateSignature(messageHash);
	}
}
