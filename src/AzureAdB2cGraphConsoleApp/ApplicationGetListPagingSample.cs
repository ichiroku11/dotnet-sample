using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// アプリケーション一覧をページングで取得
// 題材（取得するデータ）は何でもよいが
public class ApplicationGetListPagingSample(GraphServiceClient client, ILogger<SampleBase> logger)
	: SampleBase(client, logger) {

	private void LogInformation(IEnumerable<Application> apps) {
		foreach (var app in apps) {
			LogInformation(new {
				// アプリケーションID（クライアントID）
				app.AppId,
				// アプリケーションの表示名
				app.DisplayName,
			});
		}
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		var nextLink = "";

		{
			var response = await client.Applications.GetAsync(config => {
				// ページングが発生するように、一度に取得する件数を少なくする
				config.QueryParameters.Top = 2;
			});
			LogInformation(response?.Value ?? []);

			nextLink = response?.OdataNextLink;
		}

		while (!string.IsNullOrEmpty(nextLink)) {
			LogInformation(new { nextLink });

			// 次のページのデータを取得する
			var response = await client.Applications
				.WithUrl(nextLink)
				.GetAsync();
			LogInformation(response?.Value ?? []);

			nextLink = response?.OdataNextLink;
		}
	}
}
