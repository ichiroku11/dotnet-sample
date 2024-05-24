using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleLib.Hosting;

namespace AzureAdB2cGraphConsoleApp;

public class SampleService(IHost host, IHostApplicationLifetime lifetime, ILogger<SampleService> logger)
	: OnceHostedService(host, lifetime, logger) {

	protected override async Task RunAsync(IServiceProvider services) {
		// Application.Read.Allが必要
		// アプリケーション一覧取得
		await services.RunSampleAsync<ApplicationGetListSample>();

		// AuditLog.Read.Allが必要
		// 監査ログ一覧取得
		//await services.RunSampleAsync<DirectoryAuditGetListSample>();

		// たしか「User.ReadWrite.All」のアクセス許可が必要
		// ユーザー取得
		//await services.RunSampleAsync<GraphGetUserSample>();

		// ユーザー一覧取得
		//await services.RunSampleAsync<GraphGetUserListSample>();

		// ユーザー作成
		//await services.RunSampleAsync<GraphCreateUserSample>();

		// ユーザー更新（アカウントの有効・無効）
		//await services.RunSampleAsync<GraphUpdateUserAccountEnabledSample>();

		// ユーザー更新（カスタム属性）
		//await services.RunSampleAsync<GraphUpdateUserCustomAttributeSample>();

		// ユーザー更新（パスワードリセット）
		//await services.RunSampleAsync<GraphUpdateUserForceChangePasswordSample>();

		// ユーザー更新（サインインメールアドレス）
		//await services.RunSampleAsync<GraphUpdateUserSignInMailAddressSample>();
	}
}
