using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Users;

namespace AzureAdB2cGraphConsoleApp;

// ユーザーを操作するサンプル
public abstract class UserSampleBase(GraphServiceClient client, ILogger<SampleBase> logger, IOptions<GraphServiceOptions> options)
	: SampleBase(client, logger) {

	private readonly GraphServiceOptions _options = options.Value;
	private readonly CustomAttributeHelper _customAttributeHelper = new(options.Value.ExtensionAppClientId);

	// テナントID（作成時に必要）
	protected string TenantId => _options.TenantId;

	// カスタム属性の名前を取得
	protected string GetCustomAttributeFullName(string attributeName) => _customAttributeHelper.GetFullName(attributeName);

	// ユーザー一覧
	protected UsersRequestBuilder Users => Client.Users;
}
