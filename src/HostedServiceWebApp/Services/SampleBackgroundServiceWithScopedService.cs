namespace HostedServiceWebApp.Services;

// AddScopedしたサービスをコンストラクタインジェクションした場合
// プログラム起動時に例外が発生する
// InvalidOperationException:
// Cannot consume scoped service 'HostedServiceWebApp.Services.SampleScopedService' from singleton 'Microsoft.Extensions.Hosting.IHostedService'.
/*
public class SampleBackgroundServiceWithScopedService(
	ILogger<SampleBackgroundServiceWithScopedService> logger,
	SampleScopedService service)
	: BackgroundService {
	private readonly ILogger _logger = logger;
	private readonly SampleScopedService _service = service;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		_logger.LogInformation($"{nameof(ExecuteAsync)} Start");

		await _service.ActionAsync();

		_logger.LogInformation($"{nameof(ExecuteAsync)} End");
	}
}
*/

// AddScopedで追加されたサービスを呼び出すBackgroundService
// 参考
// https://stackoverflow.com/questions/48368634/how-should-i-inject-a-dbcontext-instance-into-an-ihostedservice
public class SampleBackgroundServiceWithScopedService(
	ILogger<SampleBackgroundServiceWithScopedService> logger,
	IServiceProvider serviceProvider)
	: BackgroundService {
	private readonly ILogger _logger = logger;
	private readonly IServiceProvider _serviceProvider = serviceProvider;

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		_logger.LogInformation($"{nameof(ExecuteAsync)} Start");

		/*
		// スコープを使わないと例外が発生する
		// 例外
		// System.InvalidOperationException:
		// 'Cannot resolve scoped service 'HostedServiceWebApp.Services.SampleScopedService' from root provider.'
		var service = _serviceProvider.GetRequiredService<SampleScopedService>();
		await service.ActionAsync();
		*/

		// スコープを使うとAddScopedで追加したサービスを利用できる
		using (var scope = _serviceProvider.CreateScope()) {
			var service = scope.ServiceProvider.GetRequiredService<SampleScopedService>();
			await service.ActionAsync();
		}
		// 別解はIServiceScopeFactory.CreateScope
		// https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.dependencyinjection.iservicescopefactory.createscope

		_logger.LogInformation($"{nameof(ExecuteAsync)} End");
	}
}
