using StackExchange.Redis;
using System.Text.Json;

namespace AzureCacheForRedisConsoleApp;

public static class DatabaseExtensions {
	private static readonly JsonSerializerOptions _options
		= new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			PropertyNameCaseInsensitive = true,
		};

	public static async Task<string> ExecutePingAsync(this IDatabase database) {
		var result = await database.ExecuteAsync("PING");

		return (string)result;
	}

	public static async Task<TValue?> GetAsync<TValue>(this IDatabase database, string key) {
		var json = await database.StringGetAsync(key);

		return JsonSerializer.Deserialize<TValue>(json, _options);
	}

	public static Task<bool> SetAsync<TValue>(this IDatabase database, string key, TValue value) {
		var json = JsonSerializer.Serialize(value, _options);

		return database.StringSetAsync(key, json);
	}
}
