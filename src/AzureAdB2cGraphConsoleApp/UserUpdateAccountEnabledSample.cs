using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// ユーザーを無効にする
public class UserUpdateAccountEnabledSample(GraphServiceClient client, ILogger<SampleBase> logger, IOptions<GraphServiceOptions> options)
	: UserSampleBase(client, logger, options) {
	protected override async Task RunCoreAsync() {
		// IDを指定
		var id = "{id}";

		// 更新
		var userToUpdate = new User {
			// アカウントを無効にする
			AccountEnabled = false,
		};
		await Client.Users[id].PatchAsync(userToUpdate);

		// 取得して確認
		var userUpdated = await Client.Users[id].GetAsync(config => {
			config.QueryParameters.Select = ["accountEnabled"];
		});
		if (userUpdated is not null) {
			LogInformation(userUpdated);
		}
	}
}
