using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace AzureAdB2cConsoleApp {
	public class GraphSample {
		private readonly IConfiguration _config;

		public GraphSample(IConfiguration config) {
			_config = config;
		}

		public async Task RunAsync() {
			var confidentialClientApp = ConfidentialClientApplicationBuilder
				.Create(_config["ClientId"])
				.WithTenantId(_config["TenantId"])
				.WithClientSecret(_config["ClientSecret"])
				.Build();
			var clientCredentialProvider = new ClientCredentialProvider(confidentialClientApp);

			var client = new GraphServiceClient(clientCredentialProvider);

			// ユーザ一覧を取得
			var result = await client.Users
				.Request()
				.Select(user => new {
					user.Id,
					user.DisplayName,
					user.Surname,
					user.GivenName,
					user.Mail,
					// todo:
					//user.Extensions
				})
				.GetAsync();

			foreach (var user in result.CurrentPage) {
				Console.WriteLine($"{user.Id}, {user.Surname} {user.GivenName} {user.Mail}");
				// todo: Extensions
				/*
				foreach (var extension in user.Extensions) {
				Console.WriteLine(extension);
				}
				*/
			}
		}
	}
}
