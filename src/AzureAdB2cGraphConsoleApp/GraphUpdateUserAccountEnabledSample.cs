using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// ユーザーを無効にする
public class GraphUpdateUserAccountEnabledSample(IConfiguration config, ILogger<GraphSampleBase> logger)
	: GraphSampleBase(config, logger) {
	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// IDを指定
		var id = "{id}";

		// 更新
		var userToUpdate = new User {
			// アカウントを無効にする
			AccountEnabled = false,
		};
		await client.Users[id].PatchAsync(userToUpdate);

		// 取得して確認
		var userUpdated = await client.Users[id].GetAsync(config => {
			config.QueryParameters.Select = ["accountEnabled"];
		});
		if (userUpdated is not null) {
			ShowUser(userUpdated);
		}
	}
}
