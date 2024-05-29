using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// ユーザー一覧をカスタム属性付きで取得
// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management/blob/master/src/Services/UserService.cs
// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http
// 関連リソース
// https://docs.microsoft.com/ja-jp/graph/api/resources/user?view=graph-rest-1.0
// https://docs.microsoft.com/ja-jp/graph/api/resources/objectidentity?view=graph-rest-1.0
// ログインのメールアドレスは、identitiesコレクション内のsignInTypeが"emailAddress"のissurAssignedIdに含まれる様子
public class UserGetListSample(IConfiguration config, ILogger<SampleBase> logger)
	: SampleBase(config, logger) {
	protected override async Task RunCoreAsync(GraphServiceClient client) {
		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);

		// ユーザー一覧を取得
		var response = await client.Users.GetAsync(config => {
			config.QueryParameters.Select = [
				// アカウントが有効かどうか
				"accountEnabled",
				// 作成日時
				"createdDateTime",
				// 表示名
				"displayName",
				// 名
				"givenName",
				// ID（GUID）
				"id",
				// ログインのメールアドレスが含まれる
				"identities",
				// 最後にパスワードを変更した日時またはパスワードが生成された日時
				"lastPasswordChangeDateTime",
				// メールのエイリアス
				"mailNickname",
				// パスワードプロファイル（forceChangePasswordNextSignInなどが含まれる）
				"passwordProfile",
				// 性
				"surname",
				// ユーザープリンシパル名（UPN）
				"userPrincipalName",
				// カスタム属性
				attributeName,
			];

			// 以下、フィルターのサンプル

			// idが指定した値のどれか
			//config.QueryParameters.Filter = "id in ('{id1}', '{id2}')";

			// surNameが指定した値ではじまる
			//config.QueryParameters.Filter = "startsWith(surName, '{keyword}')";

			// エラー
			// surNameのcontainsどうもサポートしていない？
			//config.QueryParameters.Filter = "contains(surName, '{keyword}')";

			// 指定したサインイン名でフィルター
			// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http#example-2-get-a-user-account-using-a-sign-in-name
			//config.QueryParameters.Filter = "identities/any(c:c/issuerAssignedId eq '{signInName}' and c/issuer eq '{tenant}')";
			// エラー
			// issuerとissuerAssignedIdどちらも指定する必要があるためか
			//config.QueryParameters.Filter = "identities/any(c:c/issuerAssignedId eq '{signInName}')";
			// エラー
			//config.QueryParameters.Filter = "identities/any(c:startsWith(c/issuerAssignedId, '{signInName}') and c/issuer eq '{tenant}')";
		});

		foreach (var user in response?.Value ?? []) {
			LogInformation(user);
		}
	}
}
