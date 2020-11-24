using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleTest.AspNetCore {
	public static class HttpRequestExtensions {
		private const string _schemeDelimiter = "://";

		// 参考
		// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.extensions.urihelper?view=aspnetcore-3.1
		// アプリケーションURLを取得
		public static string GetAppUrl(this HttpRequest request) {
			var scheme = request.Scheme ?? "";
			var host = request.Host.Value ?? "";
			var pathBase = request.PathBase.Value ?? "";

			return new StringBuilder()
				.Append(scheme)
				.Append(_schemeDelimiter)
				.Append(host)
				.Append(pathBase)
				.ToString();
		}

		// Ajaxかどうか
		public static bool IsAjax(this HttpRequest request) {
			// HeaderNames.XRequestedWithが定義されそう
			return string.Equals(
				request.Headers["X-Requested-With"],
				"XMLHttpRequest",
				StringComparison.Ordinal);
		}
	}
}
