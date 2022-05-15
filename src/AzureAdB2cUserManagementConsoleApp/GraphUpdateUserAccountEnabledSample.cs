using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cUserManagementConsoleApp;

// ユーザーのカスタム属性を更新
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
		await client.Users[id]
			.Request()
			.UpdateAsync(userToUpdate);

		// 取得して確認
		var userUpdated = await client.Users[id]
			.Request()
			.Select("accountEnabled")
			.GetAsync();
		ShowUser(userUpdated);
	}
}
