using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleLib.AspNetCore;

public static class HttpRequestExtensions {
	private const string _schemeDelimiter = "://";

	// 参考
	// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.extensions.urihelper?view=aspnetcore-3.1
	/// <summary>
	/// アプリケーションURLを取得
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
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

	/// <summary>
	/// Ajaxかどうか
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	public static bool IsAjax(this HttpRequest request) {
		// HeaderNames.XRequestedWithが定義されそう
		return string.Equals(
			request.Headers["X-Requested-With"],
			"XMLHttpRequest",
			StringComparison.Ordinal);
	}
}
