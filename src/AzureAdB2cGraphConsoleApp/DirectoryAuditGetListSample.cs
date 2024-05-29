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
public class DirectoryAuditGetListSample(IConfiguration config, ILogger<SampleBase> logger)
	: SampleBase(config, logger) {

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// AuditLog.Read.Allが必要
		// https://learn.microsoft.com/ja-jp/graph/permissions-reference#auditlogreadall
		var response = await client.AuditLogs.DirectoryAudits.GetAsync(config => {
			// 「IDトークンの発行」=「サインイン」と判断してよさげ
			config.QueryParameters.Filter = "activityDisplayName eq 'Issue an id_token to the application'";

			// ユーザーのオブジェクトIDでもフィルターする場合
			/*
			var activity = "Issue an id_token to the application";
			var objectId = "";
			config.QueryParameters.Filter = $"activityDisplayName eq '{activity}' and targetResources/any(s:s/id eq '{objectId}')";
			*/

			// 指定しなくてもactivityDateTimeの降順になっているようだが、公開ドキュメントからは探せず
			// 明示的に指定するほうが無難と判断
			config.QueryParameters.Orderby = ["activityDateTime desc"];
		});

		foreach (var audit in response?.Value ?? Enumerable.Empty<DirectoryAudit>()) {
			LogInformation(new {
				// アクティビティが実行された日時（UTC）
				// サインイン日時とする
				audit.ActivityDateTime,
				// アクティビティ名：'Issue an id_token to the application'
				audit.ActivityDisplayName,
				// アクティビティの結果
				audit.Result,
				// アクティビティを開始したアプリケーション
				// サインインしたアプリケーションのクライアントID
				audit.InitiatedBy?.App?.ServicePrincipalName,
				// ターゲットリソース
				// サインインしたユーザーのID
				// たぶん1つなんだろうけどコレクションなので配列として出力
				objectIds = audit.TargetResources?.Select(resource => resource.Id),
			});
		}
	}
}
