using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCacheForRedisConsoleApp {
	public class RedisSample {
		private readonly string _connectionString;
		private readonly ILogger _logger;

		public RedisSample(IConfiguration config, ILogger<RedisSample> logger) {
			_connectionString = config.GetConnectionString("Redis");
			_logger = logger;

		}

		private async Task PingAsync(IDatabase database) {
			// "PING"コマンドを実行
			var command = "PING";
			_logger.LogInformation(command);
			var result = await database.ExecuteAsync(command);

			// PONG
			_logger.LogInformation((string)result);
		}

		public async Task RunAsync() {
			_logger.LogInformation(nameof(RunAsync));

			// https://docs.microsoft.com/ja-jp/azure/azure-cache-for-redis/cache-dotnet-core-quickstart
			var options = ConfigurationOptions.Parse(_connectionString);

			// ConnectionMultiplexerはアプリケーション全体で共有する
			var multiplexer = await ConnectionMultiplexer.ConnectAsync(options);

			var database = multiplexer.GetDatabase();

			await PingAsync(database);
		}
	}
}
