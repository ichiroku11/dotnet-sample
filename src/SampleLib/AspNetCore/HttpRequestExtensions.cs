using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
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
		return string.Equals(
			request.Headers[HeaderNames.XRequestedWith],
			"XMLHttpRequest",
			StringComparison.Ordinal);
	}
}
