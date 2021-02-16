using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthnWebApp {
	public class BasicAuthenticationCredentialsDecoder {
		// デコード
		// ユーザ名とパスワードを取り出す
		public bool TryDecode(string encodedCredentials, out string userName, out string password) {
			userName = null;
			password = null;

			byte[] credentialsBytes;
			try {
				credentialsBytes = Convert.FromBase64String(encodedCredentials);
			} catch (FormatException) {
				return false;
			}

			// The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
			// However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
			// used in practice and defines behavior only for ASCII.
			var encoding = Encoding.ASCII;
			// Make a writable copy of the encoding to enable setting a decoder fallback.
			encoding = (Encoding)encoding.Clone();
			// Fail on invalid bytes rather than silently replacing and continuing.
			encoding.DecoderFallback = DecoderFallback.ExceptionFallback;

			string credentials;
			try {
				credentials = encoding.GetString(credentialsBytes);
			} catch (DecoderFallbackException) {
				return false;
			}

			if (string.IsNullOrEmpty(credentials)) {
				return false;
			}

			var colonIndex = credentials.IndexOf(':');
			if (colonIndex == -1) {
				return false;
			}

			userName = credentials.Substring(0, colonIndex);
			password = credentials.Substring(colonIndex + 1);
			return true;
		}
	}
}
