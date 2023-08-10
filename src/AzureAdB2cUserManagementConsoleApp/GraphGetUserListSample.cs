using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cUserManagementConsoleApp;

// ユーザー一覧をカスタム属性付きで取得
// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management/blob/master/src/Services/UserService.cs
// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http
// 関連リソース
// https://docs.microsoft.com/ja-jp/graph/api/resources/user?view=graph-rest-1.0
// https://docs.microsoft.com/ja-jp/graph/api/resources/objectidentity?view=graph-rest-1.0
// ログインのメールアドレスは、identitiesコレクション内のsignInTypeが"emailAddress"のissurAssignedIdに含まれる様子
public class GraphGetUserListSample : GraphSampleBase {
	public GraphGetUserListSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		await Task.CompletedTask;

		// todo:
#if false
		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);

		var select = string.Join(',', new[] {
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
		});

		// ユーザー一覧を取得
		var result = await client.Users
			.Request()
			.Select(select)
			.GetAsync();

		// 以下フィルタのサンプル（SelectとGetAsyncの間で呼び出す）
		// idが指定した値のどれか
		//.Filter("id in ('{id1}', '{id2}')")

		// surNameが指定した値ではじまる
		//.Filter("startsWith(surName, '{keyword}')")

		// surNameのcontainsどうもサポートしていない？
		//.Filter("contains(surName, '{keyword}')")

		// 指定したサインイン名でフィルタ
		// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http#example-2-get-a-user-account-using-a-sign-in-name
		//.Filter("identities/any(c:c/issuerAssignedId eq '{signInName}' and c/issuer eq '{tenant}')")
		// issuerとissuerAssignedIdどちらも指定する必要があるため、以下はエラー
		//.Filter("identities/any(c:c/issuerAssignedId eq '{signInName}')")

		// エラーになる
		//.Filter("identities/any(c:startsWith(c/issuerAssignedId, '{signInName}') and c/issuer eq '{tenant}')")

		foreach (var user in result.CurrentPage) {
			ShowUser(user);
		}
#endif
	}
}
