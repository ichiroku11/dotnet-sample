using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cGraphConsoleApp;

// 監査ログ一覧を取得
// 参考
// https://learn.microsoft.com/ja-jp/graph/api/resources/azure-ad-auditlog-overview?view=graph-rest-1.0
// https://learn.microsoft.com/ja-jp/graph/api/resources/directoryaudit?view=graph-rest-1.0
// https://learn.microsoft.com/ja-jp/graph/api/directoryaudit-list?view=graph-rest-1.0&tabs=http
public class GraphGetDirectoryAuditListSample(IConfiguration config, ILogger<GraphSampleBase> logger)
	: GraphSampleBase(config, logger) {

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// AuditLog.Read.Allが必要

		await Task.CompletedTask;

	}
}
