using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleLib.Hosting;

namespace AzureAdB2cUserManagementConsoleApp;

public class SampleService(IHost host, IHostApplicationLifetime lifetime, ILogger<SampleService> logger)
	: OnceHostedService(host, lifetime, logger) {
	protected override async Task RunAsync(IServiceProvider services) {
		// ユーザー取得
		//await services.GetRequiredService<GraphGetUserSample>().RunAsync();

		// ユーザー一覧取得
		await services.GetRequiredService<GraphGetUserListSample>().RunAsync();

		// ユーザー作成
		//await services.GetRequiredService<GraphCreateUserSample>().RunAsync();

		// ユーザー更新（アカウントの有効・無効）
		//await services.GetRequiredService<GraphUpdateUserAccountEnabledSample>().RunAsync();

		// ユーザー更新（カスタム属性）
		//await services.GetRequiredService<GraphUpdateUserCustomAttributeSample>().RunAsync();

		// ユーザー更新（パスワードリセット）
		//await services.GetRequiredService<GraphUpdateUserForceChangePasswordSample>().RunAsync();

		// ユーザー更新（サインインメールアドレス）
		//await services.GetRequiredService<GraphUpdateUserSignInMailAddressSample>().RunAsync();
	}
}
