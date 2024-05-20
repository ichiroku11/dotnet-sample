using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Authentication;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace AzureAdB2cGraphConsoleApp;

// 参考
// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management
// https://docs.microsoft.com/ja-jp/graph/sdks/choose-authentication-providers?tabs=CS#client-credentials-provider

public abstract class GraphSampleBase {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new() {
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			WriteIndented = true,
		};

	private readonly IConfiguration _config;
	private readonly ILogger _logger;
	private readonly CustomAttributeHelper _customAttributeHelper;

	public GraphSampleBase(IConfiguration config, ILogger<GraphSampleBase> logger) {
		_config = config;
		_logger = logger;

		_customAttributeHelper = new CustomAttributeHelper(_config["ExtensionAppClientId"] ?? throw new InvalidOperationException());
	}

	protected ILogger Logger => _logger;

	// テナントID（作成時に必要）
	protected string TenantId => _config["TenantId"] ?? throw new InvalidOperationException();

	// カスタム属性の名前を取得
	protected string GetCustomAttributeFullName(string attributeName) => _customAttributeHelper.GetFullName(attributeName);

	// JSON形式でログ出力
	protected void LogInformation<TValue>(TValue value) => _logger.LogInformation("{value}", JsonSerializer.Serialize(value, _jsonSerializerOptions));

	// サンプルの実行
	protected abstract Task RunCoreAsync(GraphServiceClient client);

	private HttpClient CreateHttpClient() {
		var handlers = GraphClientFactory.CreateDefaultHandlers();

		handlers.Add(new LoggingHandler(_logger));

		return GraphClientFactory.Create(handlers);
	}

	private IAuthenticationProvider CreateAuthenticationProvider() {
		// クレデンシャル
		var credential = new ClientSecretCredential(
			tenantId: TenantId,
			clientId: _config["ClientId"],
			clientSecret: _config["ClientSecret"],
			options: new TokenCredentialOptions {
				AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
			});

		// スコープ
		var scopes = new[] { "https://graph.microsoft.com/.default" };

		return new AzureIdentityAuthenticationProvider(credential: credential, scopes: scopes);
	}

	// サンプルの実行
	public async Task RunAsync() {
		// Graph APIを呼び出すクライアント
		var client = new GraphServiceClient(CreateHttpClient(), CreateAuthenticationProvider());

		await RunCoreAsync(client);
	}
}
