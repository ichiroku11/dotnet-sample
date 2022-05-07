using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleLib.Hosting;

namespace AzureAdB2cUserManagementConsoleApp;

public class SampleService : OnceHostedService {
	public SampleService(IHost host, IHostApplicationLifetime lifetime, ILogger<SampleService> logger)
		: base(host, lifetime, logger) {
	}

	protected override Task RunAsync(IServiceProvider services) {
		return services.GetRequiredService<GraphGetUserListSample>().RunAsync();
		// ユーザー作成
		//return services.GetRequiredService<GraphCreateUserSample>().RunAsync();
		// ユーザー更新（アカウントの有効・無効）
		//return services.GetRequiredService<GraphUpdateUserAccountEnabledSample>().RunAsync();
		// ユーザー更新（カスタム属性）
		//return services.GetRequiredService<GraphUpdateUserCustomAttributeSample>().RunAsync();
		// ユーザー更新（パスワードリセット）
		//return services.GetRequiredService<GraphUpdateUserForceChangePasswordSample>().RunAsync();
	}
}
