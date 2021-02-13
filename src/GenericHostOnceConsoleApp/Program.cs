using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GenericHostOnceConsoleApp {
	class Program {
		static async Task Main(string[] _) {
			var builder = new HostBuilder()
				.ConfigureServices(services => {
					services.Configure<ConsoleLifetimeOptions>(options => {
						// メッセージを抑制する
						options.SuppressStatusMessages = true;
					});

					// 実行するサービスを登録
					services.AddHostedService<SampleService>();
				})
				.ConfigureLogging(logging => {
					// コンソールにログを残す
					logging.AddConsole();
				});

			// ホストを開始して、サービスの実行が終わったら、ホストも終了する
			using var host = builder.Build();

			// 開始する
			await host.StartAsync();

			// 終了する
			await host.StopAsync();
		}
	}
}
