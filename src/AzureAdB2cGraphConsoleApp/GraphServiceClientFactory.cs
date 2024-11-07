using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;

namespace AzureAdB2cGraphConsoleApp;

public class GraphServiceClientFactory(IOptions<GraphServiceOptions> options, ILogger<SampleBase> logger) {
	private readonly GraphServiceOptions _options = options.Value;
	private readonly ILogger<SampleBase> _logger = logger;

	private HttpClient CreateHttpClient() {
		var handlers = GraphClientFactory.CreateDefaultHandlers();

		handlers.Add(new LoggingHandler(_logger));

		return GraphClientFactory.Create(handlers);
	}

	private AzureIdentityAuthenticationProvider CreateAuthenticationProvider() {
		// クレデンシャル
		var credential = new ClientSecretCredential(
			tenantId: _options.TenantId,
			clientId: _options.ClientId,
			clientSecret: _options.ClientSecret,
			options: new TokenCredentialOptions {
				AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
			});

		// スコープ
		var scopes = new[] { "https://graph.microsoft.com/.default" };

		return new AzureIdentityAuthenticationProvider(
			credential: credential,
			allowedHosts: null,
			observabilityOptions: null,
			isCaeEnabled: true,
			scopes: scopes);
	}

	// Graph APIを呼び出すクライアントを生成
	public GraphServiceClient Create() => new(CreateHttpClient(), CreateAuthenticationProvider());
}
