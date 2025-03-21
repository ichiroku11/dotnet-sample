using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleLib.Hosting;

namespace AzureAdB2cGraphConsoleApp;

public class SampleService(IHost host, IHostApplicationLifetime lifetime, ILogger<SampleService> logger)
	: OnceHostedService(host, lifetime, logger) {

	protected override async Task RunAsync(IServiceProvider services) {
		// 「Application.Read.All」が必要
		// アプリケーション一覧取得
		//await services.RunSampleAsync<ApplicationGetListSample>();
		//await services.RunSampleAsync<ApplicationGetListPagingSample>();

		// 「AuditLog.Read.All」が必要
		// 監査ログ一覧取得
		//await services.RunSampleAsync<DirectoryAuditGetListSample>();

		// たしか「User.ReadWrite.All」のアクセス許可が必要
		// ユーザー取得
		//await services.RunSampleAsync<UserGetSample>();

		// ユーザー一覧取得
		//await services.RunSampleAsync<UserGetListSample>();
		await services.RunSampleAsync<UserGetListSignInActivitySample>();

		// ユーザー作成
		//await services.RunSampleAsync<UserCreateSample>();

		// ユーザー更新（アカウントの有効・無効）
		//await services.RunSampleAsync<UserUpdateAccountEnabledSample>();

		// ユーザー更新（カスタム属性）
		//await services.RunSampleAsync<UserUpdateCustomAttributeSample>();

		// ユーザー更新（パスワードリセット）
		//await services.RunSampleAsync<UserUpdateForceChangePasswordSample>();

		// ユーザー更新（サインインメールアドレス）
		//await services.RunSampleAsync<UserUpdateSignInMailAddressSample>();
	}
}
