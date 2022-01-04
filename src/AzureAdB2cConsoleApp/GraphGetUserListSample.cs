using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cConsoleApp;

// ユーザー一覧をカスタム属性付きで取得
// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http
// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management/blob/master/src/Services/UserService.cs
// 関連リソース
// https://docs.microsoft.com/ja-jp/graph/api/resources/user?view=graph-rest-1.0
// https://docs.microsoft.com/ja-jp/graph/api/resources/objectidentity?view=graph-rest-1.0
public class GraphGetUserListSample : GraphSampleBase {
	public GraphGetUserListSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);

		var select = string.Join(',', new[] {
			"id",
			"surname",
			"givenName",
			"identities",
			attributeName
		});

		// ユーザー一覧を取得
		var result = await client.Users
			.Request()
			.Select(select)
			/*
			// 取得するデータを式でも表現できるが、カスタム属性は取得できないのかも
			.Select(user => new {
				user.Id,
				user.Surname,
				user.GivenName,
				user.Identities,
			})
			*/

			// 以下フィルタのサンプル
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

			.GetAsync();

		foreach (var user in result.CurrentPage) {
			ShowUser(user);
		}
	}
}
