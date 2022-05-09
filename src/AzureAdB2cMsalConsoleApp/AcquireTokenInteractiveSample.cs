using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

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

		// todo: localhostにしたいダメだった？
		//var redirectUri = $"https://{tenant}.b2clogin.com/oauth2/nativeclient";
		var redirectUri = $"http://localhost";

		var builder = PublicClientApplicationBuilder.Create(clientId)
			.WithB2CAuthority(b2cAuthorityUri)
			.WithRedirectUri(redirectUri)
			.WithLogging((level, message, containsPii) => {
				// todo: 仮
				_logger.LogInformation(message);
			});
		var app = builder.Build();

		var scopes = Enumerable.Empty<string>();
		var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
	}
}
