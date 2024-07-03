using System.Net.Http.Headers;

namespace SampleLib.AspNetCore.Authentication.Basic;

public static class HttpRequestHeadersExtensions {
	// ヘッダにBasic認証を設定する
	public static HttpRequestHeaders SetBasicAuthorization(
		this HttpRequestHeaders headers, string userName, string password) {
		var encoder = new BasicAuthenticationCredentialsEncoder();
		var parameter = encoder.Encode(userName, password);
		headers.Authorization = new AuthenticationHeaderValue("Basic", parameter);
		return headers;
	}
}
