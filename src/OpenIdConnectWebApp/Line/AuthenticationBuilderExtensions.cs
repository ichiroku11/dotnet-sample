using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace OpenIdConnectWebApp.Line;

public static class AuthenticationBuilderExtensions {
	public static AuthenticationBuilder AddOpenIdConnectLine(
		this AuthenticationBuilder builder,
		IConfiguration config,
		string authenticationScheme = LineDefaults.AuthenticationScheme) {

		builder.AddOpenIdConnect(options => {
			config.GetSection(authenticationScheme).Bind(options);

			options.MetadataAddress = LineDefaults.MetadataAddress;

			// response_typeはcode
			// 下記より
			// https://developers.line.biz/ja/docs/line-login/integrate-line-login/#making-an-authorization-request
			options.ResponseType = OpenIdConnectResponseType.Code;

			options.TokenValidationParameters.IssuerSigningKey = options.CreateIssuerSigningKey();
		});

		return builder;
	}
}
