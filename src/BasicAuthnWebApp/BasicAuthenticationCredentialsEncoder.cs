using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicAuthnWebApp {
	public class BasicAuthenticationCredentialsEncoder {
		// エンコード
		public string Encode(string userName, string password) {
			var credentialsBytes = Encoding.ASCII.GetBytes($"{userName}:{password}");
			return Convert.ToBase64String(credentialsBytes);
		}
	}
}
