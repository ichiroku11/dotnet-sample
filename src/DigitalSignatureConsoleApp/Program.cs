using System;
using System.Security.Cryptography;
using System.Text;

namespace DigitalSignatureConsoleApp {
	class Program {
		static void Main(string[] args) {
			// https://docs.microsoft.com/ja-jp/dotnet/standard/security/cryptographic-signatures

			using var rsa = RSA.Create();
			// RSAパラメーター
			var paramIncludePrivate = rsa.ExportParameters(true);
			var paramExcludePrivate = rsa.ExportParameters(false);

			// 署名対象の文字列
			var message = "あいうえお";

			// 署名を作成する
			var creator = new SignatureCreator(paramIncludePrivate);
			var signature = creator.CreateSignature(message);

			// 署名を検証する
			var verifier = new SignatureVerifier(paramExcludePrivate);
			var valid = verifier.VerifySignature(message, signature);
			Console.WriteLine(valid);
		}
	}
}
