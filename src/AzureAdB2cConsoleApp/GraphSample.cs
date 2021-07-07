using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace AzureAdB2cConsoleApp {
	public class GraphSample {
		private static readonly JsonSerializerOptions _jsonSerializerOptions
			= new() {
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
				IgnoreNullValues = true,
				//PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true,
			};

		private readonly IConfiguration _config;

		public GraphSample(IConfiguration config) {
			_config = config;
		}

		public async Task RunAsync() {
			// Graph APIを呼び出すアプリ（資格情報などを管理する）
			var confidentialClientApp = ConfidentialClientApplicationBuilder
				.Create(_config["ClientId"])
				.WithTenantId(_config["TenantId"])
				.WithClientSecret(_config["ClientSecret"])
				.Build();

			// 認証プロバイダー
			// https://docs.microsoft.com/ja-jp/graph/sdks/choose-authentication-providers?tabs=CS#client-credentials-provider
			var clientCredentialProvider = new ClientCredentialProvider(confidentialClientApp);

			// Graph APIを呼び出すクライアント
			var client = new GraphServiceClient(clientCredentialProvider);

			// ユーザ一覧を取得
			var result = await client.Users
				.Request()
				.Select(user => new {
					user.Id,
					user.Surname,
					user.GivenName,
					user.Mail,
				})
				.GetAsync();

			foreach (var user in result.CurrentPage) {
				Console.WriteLine(JsonSerializer.Serialize(user, _jsonSerializerOptions));
			}
		}
	}
}
