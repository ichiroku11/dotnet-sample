using System.Text;

namespace BasicAuthnWebApp;

public class BasicAuthenticationCredentialsEncoder {
	// エンコード
	public string Encode(string userName, string password) {
		var credentialsBytes = Encoding.ASCII.GetBytes($"{userName}:{password}");
		return Convert.ToBase64String(credentialsBytes);
	}
}
