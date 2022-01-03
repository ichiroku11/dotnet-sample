using System.Security.Cryptography;
using System.Text;

namespace DigitalSignatureConsoleApp;

public class MessageHasher {
	public byte[] Hash(string message) {
		using var sha256 = SHA256.Create();
		return sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
	}
}
