using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// ユーザー一覧をサインインアクティビティとあわせて取得
// 2024年5月、signinactivity.lastSuccessfulSignInDateTime（最新サインイン日時）がGA
// 参考
// https://learn.microsoft.com/ja-jp/entra/fundamentals/whats-new#general-availability---lastsuccessfulsignin
// https://learn.microsoft.com/ja-jp/graph/api/resources/signinactivity?view=graph-rest-1.0
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

		// 最新の成功したサインイン日時を取得
		static DateTime? getLastSuccessfulSignInDateTime(SignInActivity? signInActivity) {
			if (signInActivity is null) {
				return null;
			}

			// Azure.Identity 1.11.3では、専用のプロパティはなくAdditionalDataに含まれている
			return signInActivity.AdditionalData.TryGetValue("lastSuccessfulSignInDateTime", out var value)
				? value as DateTime?
				: null;
		}

		foreach (var user in response?.Value ?? []) {
			LogInformation(new {
				user.Id,
				user.Surname,
				user.GivenName,

				// 最新の成功したサインイン日時
				LastSuccessfulSignInDateTime = getLastSuccessfulSignInDateTime(user.SignInActivity),

				// 最新のサインインを試みた日時（成功または失敗）
				user.SignInActivity?.LastSignInDateTime,

				// 非対話型の最新のサインインを試みた日時（成功または失敗）
				user.SignInActivity?.LastNonInteractiveSignInDateTime,
			});
		}
	}
}
