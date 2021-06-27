using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAdB2cWebApp {
	public class OpenIdConnectEventHandlers {
		// Microsoft.Identity.Web.UIを使わず動きを確認したいため、
		// 以下を参考にリダイレクト先のURLを変更する
		// https://github.com/AzureAD/microsoft-identity-web/blob/cf42d50132050d8aedac14038d56e38806fa6d94/src/Microsoft.Identity.Web/AzureADB2COpenIDConnectEventHandlers.cs#L61-L99
		// https://github.com/AzureAD/microsoft-identity-web/blob/cf42d50132050d8aedac14038d56e38806fa6d94/src/Microsoft.Identity.Web/Constants/ErrorCodes.cs#L11-L14
		public Task OnRemoteFailure(RemoteFailureContext context) {
			// エラーコード
			// 参考
			// https://docs.microsoft.com/ja-jp/azure/active-directory-b2c/error-codes

			return Task.CompletedTask;
		}
	}
}
