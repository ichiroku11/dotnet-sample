using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace GenericHostConsoleApp {
	class Program {
		static async Task Main(string[] _) {
			Console.WriteLine("Hello World!");

			var builder = new HostBuilder();


			// 以下は
			// await builder.RunConsoleAsync();
			// と同じっぽい
			builder.UseConsoleLifetime();

			using var host = builder.Build();

			await host.RunAsync();
		}
	}
}
