using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cConsoleApp;

// ユーザーを仮パスワードでリセット
// 次回サインイン時に変更が必要
internal class GraphUpdateUserForceChangePasswordSample : GraphSampleBase {

	public GraphUpdateUserForceChangePasswordSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// todo:
		var id = "{id}";

		var password = PasswordHelper.Generate(6, 6, 4);

		// パスワードを更新する必要するには「ユーザー管理者ロール」が必要
		// https://docs.microsoft.com/ja-jp/azure/active-directory-b2c/microsoft-graph-get-started?tabs=app-reg-ga#optional-grant-user-administrator-role
		// 更新
		var userToUpdate = new User {
			PasswordProfile = new() {
				// 次回のログインでパスワードを変更する
				ForceChangePasswordNextSignIn = true,
				// パスワード
				Password = password,
			},
		};
		await client.Users[id]
			.Request()
			.UpdateAsync(userToUpdate);

		Logger.LogInformation(password);

		// 取得して確認
		var userUpdated = await client.Users[id]
			.Request()
			// todo:
			.GetAsync();
		ShowUser(userUpdated);
	}
}