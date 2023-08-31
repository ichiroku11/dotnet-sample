using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Text.Json;

namespace AzureAdB2cMsalConsoleApp;

public class InMemoryTokenCache {
	private static readonly JsonSerializerOptions _options = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
	};
	private readonly ILogger _logger;
	private byte[]? _cache;

	public InMemoryTokenCache(ILogger<InMemoryTokenCache> logger) {
		_logger = logger;
	}

	private void LogInformation(string method, TokenCacheNotificationArgs args) {
		_logger.LogInformation("{method}", method);
		_logger.LogInformation("{json}", JsonSerializer.Serialize(
			new {
				args.Account?.HomeAccountId,
				args.HasStateChanged,
				args.HasTokens,
				args.SuggestedCacheKey
			},
			_options));
	}

	private Task OnBeforeAccessAsync(TokenCacheNotificationArgs args) {
		LogInformation(nameof(OnBeforeAccessAsync), args);

		// todo:
		if (_cache is null) {
			return Task.CompletedTask;
		}

		// トークンキャッシュをデシリアライズする
		args.TokenCache.DeserializeMsalV3(_cache);

		return Task.CompletedTask;
	}

	private Task OnBeforeWriteAsync(TokenCacheNotificationArgs args) {
		LogInformation(nameof(OnBeforeWriteAsync), args);

		// todo:
		return Task.CompletedTask;
	}

	private Task OnAfterAccessAsync(TokenCacheNotificationArgs args) {
		LogInformation(nameof(OnAfterAccessAsync), args);

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
