using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignatureConsoleApp {
	// 署名を検証する
	public class SignatureVerifier {
		private readonly MessageHasher _hasher;
		private readonly RSAParameters _parameters;

		public SignatureVerifier(RSAParameters parameters) {
			_hasher = new MessageHasher();
			_parameters = parameters;
		}

		// メッセージと署名を検証する
		public bool VerifySignature(string message, byte[] signature) {
			var messageHash = _hasher.Hash(message);

			using var rsa = RSA.Create(_parameters);
			var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
			deformatter.SetHashAlgorithm(HashAlgorithmName.SHA256.Name);

			return deformatter.VerifySignature(messageHash, signature);
		}
	}
}
