using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureCacheForRedisConsoleApp {
	// 参考
	// https://docs.microsoft.com/ja-jp/azure/azure-cache-for-redis/cache-dotnet-core-quickstart
	public class RedisSample {
		private readonly string _connectionString;
		private readonly ILogger _logger;

		public RedisSample(IConfiguration config, ILogger<RedisSample> logger) {
			_connectionString = config.GetConnectionString("Redis");
			_logger = logger;
		}

		public async Task RunAsync() {
			_logger.LogInformation(nameof(RunAsync));

			var options = ConfigurationOptions.Parse(_connectionString);
			// Client一覧を取得するために管理操作を許可する
			options.AllowAdmin = true;

			// ConnectionMultiplexerはアプリケーション全体で共有する
			var multiplexer = await ConnectionMultiplexer.ConnectAsync(options);

			var database = multiplexer.GetDatabase();

			_logger.LogInformation("Ping:");
			var result = await database.ExecutePingAsync();
			// PONG
			_logger.LogInformation(result);

			_logger.LogInformation("EndPoints:");
			var endpoints = multiplexer.GetEndPoints();
			foreach (var endpoint in endpoints.Cast<DnsEndPoint>()) {
				_logger.LogInformation($"{endpoint.Host}:{endpoint.Port}");
			}

			var server = multiplexer.GetServer(endpoints.First());
			_logger.LogInformation("Clients:");
			var clients = await server.ClientListAsync();
			foreach (var client in clients) {
				_logger.LogInformation(client.Raw);
			}
		}
	}
}
