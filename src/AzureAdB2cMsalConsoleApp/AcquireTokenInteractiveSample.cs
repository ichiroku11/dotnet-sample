using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;

namespace AzureAdB2cMsalConsoleApp;

public class AcquireTokenInteractiveSample {
	private readonly IConfiguration _config;
	private readonly ILogger _logger;

	public AcquireTokenInteractiveSample(IConfiguration config, ILogger<AcquireTokenInteractiveSample> logger) {
		_config = config;
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
			.WithB2CAuthority(b2cAuthorityUri)
			//.WithRedirectUri(redirectUri)
			.WithDefaultRedirectUri()
			.WithLogging((level, message, containsPii) => {
				_logger.LogInformation($"{nameof(LogCallback)}: {message}");
			});
		var app = builder.Build();

		app.UserTokenCache.SetBeforeAccessAsync(args => {
			_logger.LogInformation($"{nameof(ITokenCache.SetBeforeAccessAsync)}");

			return Task.CompletedTask;
		});
		app.UserTokenCache.SetBeforeWriteAsync(args => {
			_logger.LogInformation($"{nameof(ITokenCache.SetBeforeWriteAsync)}");

			return Task.CompletedTask;
		});
		app.UserTokenCache.SetAfterAccessAsync(args => {
			_logger.LogInformation($"{nameof(ITokenCache.SetAfterAccessAsync)}");

			return Task.CompletedTask;
		});

		var scopes = Enumerable.Empty<string>();
		var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

		_logger.LogInformation(result.IdToken);

		var handler = new JwtSecurityTokenHandler();
		_logger.LogInformation(handler.CanReadToken(result.IdToken).ToString());

		var token = handler.ReadJwtToken(result.IdToken);
		_logger.LogInformation(token.ToString());
	}
}
