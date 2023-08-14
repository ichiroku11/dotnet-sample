using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cUserManagementConsoleApp;

// ユーザーを無効にする
public class GraphUpdateUserAccountEnabledSample : GraphSampleBase {
	public GraphUpdateUserAccountEnabledSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// todo:
		var id = "{id}";

		// 更新
		var userToUpdate = new User {
			// アカウントを無効にする
			AccountEnabled = false,
		};
		await client.Users[id].PatchAsync(userToUpdate);

		// 取得して確認
		var userUpdated = await client.Users[id].GetAsync(config => {
			config.QueryParameters.Select = new[] { "accountEnabled" };
		});
		if (userUpdated is not null) {
			ShowUser(userUpdated);
		}
	}
}
