using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BasicAuthnWebApp {
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
}
