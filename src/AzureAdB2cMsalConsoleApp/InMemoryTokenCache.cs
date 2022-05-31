using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace AzureAdB2cMsalConsoleApp;

public class InMemoryTokenCache {
	private readonly ILogger _logger;
	private byte[]? _cache;

	public InMemoryTokenCache(ILogger<InMemoryTokenCache> logger) {
		_logger = logger;
	}

	private Task OnBeforeAccessAsync(TokenCacheNotificationArgs args) {
		_logger.LogInformation($"{nameof(OnBeforeAccessAsync)}");

		// todo:
		if (_cache is null) {
			return Task.CompletedTask;
		}

		// トークンキャッシュをデシリアライズする
		args.TokenCache.DeserializeMsalV3(_cache);

		return Task.CompletedTask;
	}

	private Task OnBeforeWriteAsync(TokenCacheNotificationArgs arsg) {
		_logger.LogInformation($"{nameof(OnBeforeWriteAsync)}");

		// todo:
		return Task.CompletedTask;
	}

	private Task OnAfterAccessAsync(TokenCacheNotificationArgs args) {
		_logger.LogInformation($"{nameof(OnAfterAccessAsync)}");

		// todo:
		if (!args.HasStateChanged) {
			return Task.CompletedTask;
		}

		// トークンキャッシュをシリアライズする
		_cache = args.TokenCache.SerializeMsalV3();

		return Task.CompletedTask;
	}

	// コールバックイベントをバインドする
	public void Bind(ITokenCache tokenCache) {
		// キャッシュにアクセスする前に呼び出される
		tokenCache.SetBeforeAccessAsync(OnBeforeAccessAsync);

		// キャッシュに書き込む前に呼び出される
		tokenCache.SetBeforeWriteAsync(OnBeforeWriteAsync);

		// キャッシュにアクセスした後に呼び出される
		tokenCache.SetAfterAccessAsync(OnAfterAccessAsync);
	}
}
