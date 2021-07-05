using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace AzureAdB2cConsoleApp {
	class Program {
		// todo:
		private const string _tenantId = "";
		private const string _clientId = "";
		private const string _clientSecret = "";

		static async Task Main(string[] args) {
			Console.WriteLine("Hello World!");

			var confidentialClientApp = ConfidentialClientApplicationBuilder
				.Create(_clientId)
				.WithTenantId(_tenantId)
				.WithClientSecret(_clientSecret)
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
