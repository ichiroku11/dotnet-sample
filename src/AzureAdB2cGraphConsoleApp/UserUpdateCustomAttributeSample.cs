using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// ユーザーのカスタム属性を更新
public class UserUpdateCustomAttributeSample(IConfiguration config, ILogger<SampleBase> logger)
	: SampleBase(config, logger) {
	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// IDを指定
		var id = "{id}";

		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);
		var attributeValue = new Random().Next(1, 10);

		// 更新
		var userToUpdate = new User {
			AdditionalData = new Dictionary<string, object> {
				[attributeName] = attributeValue,
			},
		};
		await client.Users[id].PatchAsync(userToUpdate);

		// 取得して確認
		var userUpdated = await client.Users[id].GetAsync(config => {
			config.QueryParameters.Select = [attributeName];
		});

		if (userUpdated is not null) {
			LogInformation(userUpdated);
		}
	}
}
