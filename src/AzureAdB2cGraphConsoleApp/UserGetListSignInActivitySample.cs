using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cGraphConsoleApp;

// ユーザー一覧をサインインアクティビティとあわせて取得
public class UserGetListSignInActivitySample(IConfiguration config, ILogger<SampleBase> logger)
	: SampleBase(config, logger) {
	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// ユーザー一覧を取得
		var response = await client.Users.GetAsync(config => {
			config.QueryParameters.Select = [
				// 名
				"givenName",
				// ID（GUID）
				"id",
				// 性
				"surname",
				// サインインアクティビティ
				"signInActivity"
			];
		});

		foreach (var user in response?.Value ?? []) {
			LogInformation(user);
		}
	}
}
