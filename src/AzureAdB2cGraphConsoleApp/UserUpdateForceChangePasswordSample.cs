using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// ユーザーを仮パスワードでリセット
// 次回サインイン時に変更が必要
public class UserUpdateForceChangePasswordSample(GraphServiceClient client, ILogger<SampleBase> logger, IOptions<GraphServiceOptions> options)
	: UserSampleBase(client, logger, options) {
	protected override async Task RunCoreAsync() {
		// IDを指定
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
		await Users[id].PatchAsync(userToUpdate);

		Logger.LogInformation("{password}", password);

		// 取得して確認
		var userUpdated = await Users[id].GetAsync();
		if (userUpdated is not null) {
			LogInformation(userUpdated);
		}
	}
}
