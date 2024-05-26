using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// アプリケーション一覧を取得
// 参考
// https://learn.microsoft.com/ja-jp/graph/api/resources/application?view=graph-rest-1.0
public class ApplicationGetListSample(IConfiguration config, ILogger<SampleBase> logger)
	: SampleBase(config, logger) {

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// Application.Read.Allが必要
		// https://learn.microsoft.com/ja-jp/graph/permissions-reference#applicationreadall
		var response = await client.Applications.GetAsync(config => {
			// アプリケーションは並び替えをサポートしていない様子
			// 以下のエラーが発生する
			// Microsoft.Graph.Models.ODataErrors.ODataError: "Sorting not supported for 'Application'."
			//config.QueryParameters.Orderby = ["displayName asc"];
		});

		foreach (var app in response?.Value ?? Enumerable.Empty<Application>()) {
			LogInformation(new {
				// アプリケーションID（クライアントID）
				app.AppId,
				// アプリケーションの表示名
				app.DisplayName,
			});
		}
	}
}
