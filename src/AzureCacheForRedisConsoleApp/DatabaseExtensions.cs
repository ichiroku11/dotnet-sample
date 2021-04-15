using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AzureCacheForRedisConsoleApp {
	public static class DatabaseExtensions {
		public static async Task<string> ExecutePingAsync(this IDatabase database) {
			var result = await database.ExecuteAsync("PING");

			return (string)result;
		}
	}
}
