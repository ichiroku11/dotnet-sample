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
	private readonly ILogger _logger;

	public AcquireTokenInteractiveSample(
		IConfiguration config,
		IHttpClientFactory factory,
		ILogger<AcquireTokenInteractiveSample> logger) {
		_config = config;
		_factory = factory;
		_logger = logger;
	}

	private async Task CallApiAsync(string accessToken) {
		var client = _factory.CreateClient();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		// todo:
		await Task.CompletedTask;
		/*
		var url = "https://localhost:7237/api/value";
		var response = await client.GetAsync(url);
		var values = await response
			.EnsureSuccessStatusCode()
			.Content
			.ReadFromJsonAsync<IEnumerable<string>>();

		foreach (var value in values ?? Enumerable.Empty<string>()) {
			Console.WriteLine(value);
		}
		*/
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
			.WithB2CAuthority(b2cAuthorityUri)
			//.WithRedirectUri(redirectUri)
			.WithDefaultRedirectUri()
			.WithLogging((level, message, containsPii) => {
				_logger.LogInformation($"{nameof(LogCallback)}: {message}");
			});
		var app = builder.Build();

		// キャッシュにアクセスする前に呼び出される
		app.UserTokenCache.SetBeforeAccessAsync(args => {
			_logger.LogInformation($"{nameof(ITokenCache.SetBeforeAccessAsync)}");

			// todo: キャッシュからデシリアライズする

			return Task.CompletedTask;
		});
		app.UserTokenCache.SetBeforeWriteAsync(args => {
			_logger.LogInformation($"{nameof(ITokenCache.SetBeforeWriteAsync)}");

			return Task.CompletedTask;
		});

		// キャッシュにアクセスした後に呼び出される
		app.UserTokenCache.SetAfterAccessAsync(args => {
			_logger.LogInformation($"{nameof(ITokenCache.SetAfterAccessAsync)}");

			// todo: キャッシュにシリアライズする

			return Task.CompletedTask;
		});

		var scopes = Enumerable.Empty<string>();
		var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
		var handler = new JwtSecurityTokenHandler();
		var token = handler.ReadJwtToken(result.IdToken);

		_logger.LogInformation(result.IdToken);
		//_logger.LogInformation(handler.CanReadToken(result.IdToken).ToString());
		_logger.LogInformation(token.ToString());

		await CallApiAsync(result.AccessToken);
	}
}
