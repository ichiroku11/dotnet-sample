using AzureAdB2cUserManagementConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

// ユーザーのサインインメールアドレスを変更する
// 参考
// - https://learn.microsoft.com/en-us/answers/questions/291598/azure-ad-b2c-update-email-address-clone-existing-u
internal class GraphUpdateUserSignInMailAddressSample(IConfiguration config, ILogger<GraphSampleBase> logger)
	: GraphSampleBase(config, logger) {
	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// IDを指定
		var id = "{id}";
		// 変更後のメールアドレスを指定
		var mail = "{mail}";

		var userToUpdate = new User {
			Identities = new List<ObjectIdentity> {
				// サインイン情報を設定する
				// https://docs.microsoft.com/ja-jp/graph/api/resources/objectidentity
				new() {
					Issuer = TenantId,
					IssuerAssignedId = mail,
					// メールアドレスでログインする
					SignInType = "emailAddress",
				},
			},
		};

		Logger.LogInformation("{mail}", mail);

		await client.Users[id].PatchAsync(userToUpdate);

		// 取得して確認
		var userUpdated = await client.Users[id].GetAsync(
			config => {
				config.QueryParameters.Select = new[] { "identities" };
			});
		if (userUpdated is not null) {
			ShowUser(userUpdated);
		}
	}
}
