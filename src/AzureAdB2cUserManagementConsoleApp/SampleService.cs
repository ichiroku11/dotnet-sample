using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzureAdB2cUserManagementConsoleApp;

public class SampleService : IHostedService {
	private readonly IServiceProvider _services;
	private readonly IHostApplicationLifetime _lifetime;
	private readonly ILogger _logger;

	public SampleService(
		IHost host,
		IHostApplicationLifetime lifetime,
		ILogger<SampleService> logger) {
		_services = host.Services;
		_lifetime = lifetime;
		_logger = logger;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		_logger.LogInformation(nameof(StartAsync));

		_lifetime.ApplicationStarted.Register(async () => {
			try {
				await _services.GetRequiredService<GraphGetUserListSample>().RunAsync();
				// ユーザー作成
				//await _services.GetRequiredService<GraphCreateUserSample>().RunAsync();
				// ユーザー更新（アカウントの有効・無効）
				//await _services.GetRequiredService<GraphUpdateUserAccountEnabledSample>().RunAsync();
				// ユーザー更新（カスタム属性）
				//await _services.GetRequiredService<GraphUpdateUserCustomAttributeSample>().RunAsync();
				// ユーザー更新（パスワードリセット）
				//await _services.GetRequiredService<GraphUpdateUserForceChangePasswordSample>().RunAsync();
			} catch (Exception) {
				throw;
			}

			_lifetime.StopApplication();
		});

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation(nameof(StopAsync));
		return Task.CompletedTask;
	}
}
