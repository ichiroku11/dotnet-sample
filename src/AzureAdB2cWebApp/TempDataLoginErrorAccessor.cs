using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Identity.Web;

namespace AzureAdB2cWebApp;

// デフォルトの実装では、Development環境以外では何もしない様子
// https://github.com/AzureAD/microsoft-identity-web/blob/195328cc509e3d964bb47f1877a8946b6f136f81/src/Microsoft.Identity.Web/WebAppExtensions/MicrosoftIdentityWebAppAuthenticationBuilderExtensions.cs#L261-L275
// Production環境でもエラーメッセージを取得できるように下記を参考に実装する
// https://github.com/AzureAD/microsoft-identity-web/blob/master/src/Microsoft.Identity.Web/TempDataLoginErrorAccessor.cs
// TempDataにクッキーを使っている場合はよいが、
// セッションを使っている場合は、もしかするとミドルウェアの関係でエラーになるかも
public class TempDataLoginErrorAccessor(
	ILogger<TempDataLoginErrorAccessor> logger,
	ITempDataDictionaryFactory factory)
	: ILoginErrorAccessor {
	private readonly ILogger<TempDataLoginErrorAccessor> _logger = logger;
	private readonly ITempDataDictionaryFactory _factory = factory;

	private const string _tempDataKey = $"{nameof(AzureAdB2cWebApp)}.{nameof(TempDataLoginErrorAccessor)}";

	public bool IsEnabled => true;

	public string? GetMessage(HttpContext context) {
		_logger.LogInformation("{method}", nameof(GetMessage));

		var tempData = _factory.GetTempData(context);

		if (tempData.TryGetValue(_tempDataKey, out var result) && result is string message) {
			return message;
		}

		return null;
	}

	public void SetMessage(HttpContext context, string? message) {
		_logger.LogInformation("{method}: {message}", nameof(SetMessage), message);

		if (message is null) {
			return;
		}

		var tempData = _factory.GetTempData(context);

		tempData[_tempDataKey] = message;
		tempData.Save();
	}
}
