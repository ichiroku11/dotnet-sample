using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cConsoleApp;

// ユーザーをカスタム属性付きで取得
public class GraphGetUserSample : GraphSampleBase {
	public GraphGetUserSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// IDを指定
		var id = "{id}";

		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);

		var select = string.Join(',', new[] {
				"id",
				"surname",
				"givenName",
				"identities",
				attributeName,
			});
		// ユーザーをID指定で取得
		var user = await client.Users[id]
			.Request()
			.Select(select)
			/*
			// 取得するデータを式でも表現できるが、カスタム属性は取得できないのかも
			.Select(user => new {
				user.Id,
				user.Surname,
				user.GivenName,
				user.Identities,
			})
			*/
			.GetAsync();

		ShowUser(user);
	}
}
