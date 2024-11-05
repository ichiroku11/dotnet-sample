using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

namespace AzureAdB2cGraphConsoleApp;

// ユーザーをカスタム属性付きで取得
public class UserGetSample(GraphServiceClient client, ILogger<SampleBase> logger, IOptions<GraphServiceOptions> options)
	: UserSampleBase(client, logger, options) {
	protected override async Task RunCoreAsync() {
		// IDを指定
		var id = "{id}";

		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);

		// ユーザーをID指定で取得
		var user = await Client.Users[id].GetAsync(config => {
			config.QueryParameters.Select = [
				"id",
				"surname",
				"givenName",
				"identities",
				attributeName,
			];
		});

		if (user is not null) {
			LogInformation(user);
		}
	}
}
