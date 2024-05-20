using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cGraphConsoleApp;

// ユーザーをカスタム属性付きで取得
public class GraphGetUserSample(IConfiguration config, ILogger<GraphSampleBase> logger)
	: GraphSampleBase(config, logger) {
	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// IDを指定
		var id = "{id}";

		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);

		// ユーザーをID指定で取得
		var user = await client.Users[id].GetAsync(config => {
			config.QueryParameters.Select = [
				"id",
				"surname",
				"givenName",
				"identities",
				attributeName,
			];
		});

		if (user is not null) {
			ShowUser(user);
		}
	}
}
