using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace AzureCacheForRedisConsoleApp {
	class Program {
		static async Task Main(string[] args) {
			Console.WriteLine("Hello World!");
			// https://docs.microsoft.com/ja-jp/azure/azure-cache-for-redis/cache-dotnet-core-quickstart

			var config = new ConfigurationBuilder()
				.AddUserSecrets<Program>()
				.Build();

			var connectionString = config.GetConnectionString("Redis");

			var multiplexer = await ConnectionMultiplexer.ConnectAsync(connectionString);

			var database = multiplexer.GetDatabase();

			var command = "PING";
			Console.WriteLine(command);
			var result = await database.ExecuteAsync(command);

			// PONG
			Console.WriteLine(result);
		}
	}
}
