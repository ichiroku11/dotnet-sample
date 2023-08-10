using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cUserManagementConsoleApp;

// ユーザーのカスタム属性を更新
public class GraphUpdateUserCustomAttributeSample : GraphSampleBase {
	public GraphUpdateUserCustomAttributeSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		await Task.CompletedTask;

		// todo:
#if false
		// todo:
		var id = "{id}";

		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);
		var attributeValue = new Random().Next(1, 10);

		// 更新
		var userToUpdate = new User {
			AdditionalData = new Dictionary<string, object> {
				[attributeName] = attributeValue,
			},
		};
		await client.Users[id]
			.Request()
			.UpdateAsync(userToUpdate);

		// 取得して確認
		var userUpdated = await client.Users[id]
			.Request()
			.Select(attributeName)
			.GetAsync();
		ShowUser(userUpdated);
#endif
	}
}
