using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AzureAdB2cMsalConsoleApp;

public class AcquireTokenInteractiveSample {
	private readonly IConfiguration _config;
	private readonly IHttpClientFactory _factory;
	private readonly InMemoryTokenCache _tokenCache;
	private readonly ILogger _logger;

	public AcquireTokenInteractiveSample(
		IConfiguration config,
		IHttpClientFactory factory,
		InMemoryTokenCache tokenCache,
		ILogger<AcquireTokenInteractiveSample> logger) {
		_config = config;
		_factory = factory;
		_tokenCache = tokenCache;
		_logger = logger;
	}

	public async Task RunAsync() {
		// https://docs.microsoft.com/ja-jp/azure/active-directory-b2c/integrate-with-app-code-samples
		// https://github.com/Azure-Samples/active-directory-b2c-dotnet-desktop

		var clientId = _config["ClientId"];
		var tenantName = _config["TenantName"];
		var userFlow = "B2C_1_signupsignin";

		var b2cAuthorityUri = $"https://{tenantName}.b2clogin.com/tfp/{tenantName}.onmicrosoft.com/{userFlow}";

		// redirectUriにlocalhost以外を指定すると例外になった
		//var redirectUri = $"https://{tenantName}.b2clogin.com/oauth2/nativeclient";
		// Only loopback redirect uri is supported, but https://{tenantName}.b2clogin.com/oauth2/nativeclient was found.
		// Configure http://localhost or http://localhost:port both during app registration and when you create the PublicClientApplication object.
		// See https://aka.ms/msal-net-os-browser for details
		//var redirectUri = "http://localhost";

		var builder = PublicClientApplicationBuilder.Create(clientId)
			.WithHttpClientFactory(new SimpleHttpClientFactory(_factory.CreateClient("b2c")))
			.WithB2CAuthority(b2cAuthorityUri)
			//.WithRedirectUri(redirectUri)
			.WithDefaultRedirectUri()
			.WithLogging((level, message, containsPii) => {
				_logger.LogInformation($"{nameof(LogCallback)}: {message}");
			});
		var app = builder.Build();

		_tokenCache.Bind(app.UserTokenCache);

		// スコープが空だとアクセストークンがとれない（result.AccessTokenがnullになる）
		//var scopes = Enumerable.Empty<string>();
		// 下記手順でWeb APIアプリとスコープを登録した後、
		// スコープを指定するとアクセストークンを取得できる
		// https://docs.microsoft.com/ja-jp/azure/active-directory-b2c/configure-authentication-sample-web-app-with-api?tabs=visual-studio
		var scopes = new[] {
			$"https://{tenantName}.onmicrosoft.com/webapi/test.write",
			$"https://{tenantName}.onmicrosoft.com/webapi/test.read",
		};
		var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
		var handler = new JwtSecurityTokenHandler();
		var token = handler.ReadJwtToken(result.IdToken);

		_logger.LogInformation(result.IdToken);
		//_logger.LogInformation(handler.CanReadToken(result.IdToken).ToString());
		_logger.LogInformation(token.ToString());

		var accessToken = handler.ReadJwtToken(result.AccessToken);
		_logger.LogInformation(result.AccessToken);
		_logger.LogInformation(accessToken.ToString());
	}
}
