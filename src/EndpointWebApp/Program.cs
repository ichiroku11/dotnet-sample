using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EndpointWebApp {
	public class Program {
		public static void Main(string[] args) {
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => {
					// MVCを使わない場合のEndpointとMetadataを確認する
					//webBuilder.UseStartup<SampleStartup>();

					// MVC、コントローラを使った場合のEndpointとMetadataを確認する
					webBuilder.UseStartup<ControllerStartup>();
				});
	}
}
