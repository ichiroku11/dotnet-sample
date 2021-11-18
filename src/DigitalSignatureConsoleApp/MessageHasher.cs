using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignatureConsoleApp;

public class MessageHasher {
	public byte[] Hash(string message) {
		using var sha256 = SHA256.Create();
		return sha256.ComputeHash(Encoding.UTF8.GetBytes(message));
	}
}
