using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Net;

namespace AzureCacheForRedisConsoleApp;

// 参考
// https://docs.microsoft.com/ja-jp/azure/azure-cache-for-redis/cache-dotnet-core-quickstart
public class RedisSample(IConfiguration config, ILogger<RedisSample> logger) {
	private readonly string _connectionString = config.GetConnectionString("Redis") ?? throw new InvalidOperationException();
	private readonly ILogger _logger = logger;

	private record Message(string Content);

	public async Task RunAsync() {
		_logger.LogInformation(nameof(RunAsync));

		var options = ConfigurationOptions.Parse(_connectionString);
		// Client一覧を取得するために管理操作を許可する
		options.AllowAdmin = true;

		// ConnectionMultiplexerはアプリケーション全体で共有する
		var multiplexer = await ConnectionMultiplexer.ConnectAsync(options);

		var database = multiplexer.GetDatabase();

		// PINGコマンドを実行
		_logger.LogInformation("Ping:");
		var result = await database.ExecutePingAsync();
		_logger.LogInformation("{result}", result);

		// エンドポイント一覧
		_logger.LogInformation("EndPoints:");
		var endpoints = multiplexer.GetEndPoints();
		foreach (var endpoint in endpoints.Cast<DnsEndPoint>()) {
			_logger.LogInformation("{host}:{post}", endpoint.Host, endpoint.Port);
		}

		// クライアント一覧
		var server = multiplexer.GetServer(endpoints.First());
		_logger.LogInformation("Clients:");
		var clients = await server.ClientListAsync();
		foreach (var client in clients) {
			_logger.LogInformation("{raw}", client.Raw);
		}

		// オブジェクトを設定・取得
		await database.SetAsync("test", new Message("Hello Redis!"));
		var message = await database.GetAsync<Message>("test");
		_logger.LogInformation("{action}:{content}", nameof(DatabaseExtensions.GetAsync), message?.Content);
	}
}
