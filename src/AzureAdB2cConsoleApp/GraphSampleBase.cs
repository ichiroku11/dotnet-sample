using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace AzureAdB2cConsoleApp;

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

		_customAttributeHelper = new CustomAttributeHelper(_config["ExtensionAppClientId"]);
	}

	// テナントID（作成時に必要）
	protected string TenantId => _config["TenantId"];

	// カスタム属性の名前を取得
	protected string GetCustomAttributeFullName(string attributeName) => _customAttributeHelper.GetFullName(attributeName);

	// ユーザー情報をの表示
	protected void ShowUser(User user) => Console.WriteLine(JsonSerializer.Serialize(user, _jsonSerializerOptions));

	protected abstract Task RunCoreAsync(GraphServiceClient client);

	public async Task RunAsync() {
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

		// HTTPプロバイダー
		using var httpProvider = new LoggingHttpProvider(new HttpProvider(), _logger);

		// Graph APIを呼び出すクライアント
		var client = new GraphServiceClient(credential, scopes, httpProvider);

		await RunCoreAsync(client);
	}
}
