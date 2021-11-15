using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAdB2cWebApp;

public class OpenIdConnectEventHandlers {
	// Microsoft.Identity.Web.UIを使わず動きを確認したいため、
	// 以下を参考にリダイレクト先のURLを変更する
	// https://github.com/AzureAD/microsoft-identity-web/blob/cf42d50132050d8aedac14038d56e38806fa6d94/src/Microsoft.Identity.Web/AzureADB2COpenIDConnectEventHandlers.cs#L61-L99
	// https://github.com/AzureAD/microsoft-identity-web/blob/cf42d50132050d8aedac14038d56e38806fa6d94/src/Microsoft.Identity.Web/Constants/ErrorCodes.cs#L11-L14
	public Task OnRemoteFailure(RemoteFailureContext context) {
		context.HandleResponse();

		var redirectUrl = $"{context.Request.PathBase}/account/error";
		var isOidcProtocolException = context.Failure is OpenIdConnectProtocolException;
		var message = context.Failure?.Message ?? "";
		// 参考）エラーコード
		// https://docs.microsoft.com/ja-jp/azure/active-directory-b2c/error-codes
		if (isOidcProtocolException && message.Contains("access_denied", StringComparison.OrdinalIgnoreCase)) {
			// 「キャンセル」をクリックした場合
			redirectUrl = $"{context.Request.PathBase}/";

		} else {
			var loginErrorAccessor = context.HttpContext.RequestServices.GetService<ILoginErrorAccessor>();
			loginErrorAccessor?.SetMessage(context.HttpContext, message);
		}

		context.Response.Redirect(redirectUrl);

		return Task.CompletedTask;
	}
}
