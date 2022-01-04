using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cConsoleApp;

// ユーザーをカスタム属性付きで取得
public class GraphGetUserSample : GraphSampleBase {
	public GraphGetUserSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		var id = "{id}";

		var attributeName = GetCustomAttributeFullName("TestNumber");

		var select = string.Join(',', new[] {
				"id",
				"surname",
				"givenName",
				"identities",
				attributeName
			});

		// ユーザーをID指定で取得
		var user = await client.Users[id]
			.Request()
			.Select(select)
			.GetAsync();

		ShowUser(user);
	}
}
