using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// 監査ログ一覧を取得
// 参考
// https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/view-audit-logs
// https://learn.microsoft.com/ja-jp/graph/api/directoryaudit-list?view=graph-rest-1.0&tabs=http
// https://learn.microsoft.com/ja-jp/graph/api/resources/azure-ad-auditlog-overview?view=graph-rest-1.0
// https://learn.microsoft.com/ja-jp/graph/api/resources/directoryaudit?view=graph-rest-1.0
// https://learn.microsoft.com/ja-jp/graph/api/resources/auditactivityinitiator?view=graph-rest-1.0
// https://learn.microsoft.com/ja-jp/graph/api/resources/targetresource?view=graph-rest-1.0

public class GraphGetDirectoryAuditListSample(IConfiguration config, ILogger<GraphSampleBase> logger)
	: GraphSampleBase(config, logger) {

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// AuditLog.Read.Allが必要
		// https://learn.microsoft.com/ja-jp/graph/permissions-reference#auditlogreadall
		var audits = await client.AuditLogs.DirectoryAudits.GetAsync(config => {
			// 「IDトークンの発行」=「サインイン」と判断してよさげ
			config.QueryParameters.Filter = "activityDisplayName eq 'Issue an id_token to the application'";
		});

		foreach (var audit in audits?.Value ?? Enumerable.Empty<DirectoryAudit>()) {
			LogInformation(new {
				// サインイン日時
				audit.ActivityDateTime,
				// 'Issue an id_token to the application'
				audit.ActivityDisplayName,
				// サインインしたアプリケーションのクライアントID
				audit.InitiatedBy?.App?.ServicePrincipalName,
			});
		}
	}
}
