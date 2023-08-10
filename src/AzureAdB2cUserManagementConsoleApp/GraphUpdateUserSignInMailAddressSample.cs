using AzureAdB2cUserManagementConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

// ユーザーのサインインメールアドレスを変更する
// 参考
// - https://learn.microsoft.com/en-us/answers/questions/291598/azure-ad-b2c-update-email-address-clone-existing-u
internal class GraphUpdateUserSignInMailAddressSample : GraphSampleBase {
	public GraphUpdateUserSignInMailAddressSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		await Task.CompletedTask;

		// todo:
#if false
		// todo:
		var id = "{id}";
		var mail = "{mail}";

		var userToUpdate = new User {
			Identities = new[] {
				// サインイン情報を設定する
				// https://docs.microsoft.com/ja-jp/graph/api/resources/objectidentity
				new ObjectIdentity {
					Issuer = TenantId,
					IssuerAssignedId = mail,
					// メールアドレスでログインする
					SignInType = "emailAddress",
				},
			},
		};

		Logger.LogInformation("{mail}", mail);

		await client.Users[id]
			.Request()
			.UpdateAsync(userToUpdate);

		// 取得して確認
		var userUpdated = await client.Users[id]
			.Request()
			.Select("identities")
			.GetAsync();
		ShowUser(userUpdated);
#endif
	}
}
