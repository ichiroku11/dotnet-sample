using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OpenIdConnectWebApp.Line;

public static class OpenIdConnectOptionsExtensions {
	// 署名を検証する鍵を生成する
	// ウェブログインにおける署名はHS256で、
	// 署名鍵はチャネルシークレット（クライアントシークレット）
	// 下記より
	// https://developers.line.biz/ja/docs/line-login/verify-id-token/#header
	// https://developers.line.biz/ja/docs/line-login/verify-id-token/#signature
	public static SecurityKey CreateIssuerSigningKey(this OpenIdConnectOptions options) {
		if (string.IsNullOrWhiteSpace(options.ClientSecret)) {
			throw new InvalidOperationException();
		}

		return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.ClientSecret));
	}
}
