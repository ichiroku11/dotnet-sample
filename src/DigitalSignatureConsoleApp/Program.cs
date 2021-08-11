using System;
using System.Security.Cryptography;
using System.Text;

namespace DigitalSignatureConsoleApp {

	public class MessageHasher {
		public byte[] Hash(string message) {
			using var algorithm = SHA256.Create();
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(message));
		}
	}

	public class SignatureCreator {
		private readonly MessageHasher _hasher;
		private readonly RSAParameters _parameters;

		public SignatureCreator(RSAParameters parameters) {
			_hasher = new MessageHasher();
			_parameters = parameters;
		}

		public byte[] CreateSignature(string message) {
			// todo:
			throw new NotImplementedException();
		}

	}

	public class SignatureVerifier {
		private readonly MessageHasher _hasher;
		private readonly RSAParameters _parameters;

		public SignatureVerifier(RSAParameters parameters) {
			_hasher = new MessageHasher();
			_parameters = parameters;
		}

		public bool VerifySignature(string message, byte[] signature) {
			// todo:
			throw new NotImplementedException();
		}
	}

	class Program {
		static void Main(string[] args) {
			Console.WriteLine("Hello World!");
			// https://docs.microsoft.com/ja-jp/dotnet/standard/security/cryptographic-signatures

			using var rsa = RSA.Create();

			var message = "あいうえお";

			// 署名を作成する
			var creator = new SignatureCreator(rsa.ExportParameters(true));
			var signature = creator.CreateSignature(message);

			// 署名を検証する
			var verifier = new SignatureVerifier(rsa.ExportParameters(false));
			var valid = verifier.VerifySignature(message, signature);
		}
	}
}
