using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
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

		// ユーザ一覧を取得
		// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http
		private async Task GetUsersAsync(IGraphServiceClient client) {
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
				Console.WriteLine($"{nameof(user.Id)}: {user.Id}");
				Console.WriteLine($"{nameof(user.Surname)}: {user.Surname}");
				Console.WriteLine($"{nameof(user.GivenName)}: {user.GivenName}");
				Console.WriteLine($"{nameof(user.Mail)}: {user.Mail}");
				Console.WriteLine($"Json:");
				Console.WriteLine(JsonSerializer.Serialize(user, _jsonSerializerOptions));
			}
		}

		// ユーザ一覧をカスタム属性付きで取得
		// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management/blob/master/src/Services/UserService.cs
		private async Task GetUsersAsync(IGraphServiceClient client, string extensionAppClientId) {
			var helper = new CustomAttributeHelper(extensionAppClientId);

			// カスタム属性名
			var attributeName = helper.GetFullName("TestNumber");

			var select = string.Join(',', new[] {
				"id",
				"surname",
				"givenName",
				"mail",
				attributeName
			});

			// ユーザ一覧を取得
			var result = await client.Users
				.Request()
				.Select(select)
				.GetAsync();

			foreach (var user in result.CurrentPage) {
				Console.WriteLine($"{nameof(user.Id)}: {user.Id}");
				Console.WriteLine($"{nameof(user.Surname)}: {user.Surname}");
				Console.WriteLine($"{nameof(user.GivenName)}: {user.GivenName}");
				Console.WriteLine($"{nameof(user.Mail)}: {user.Mail}");
				// カスタム属性は存在しない場合がある
				var attributeValue = user.AdditionalData.ContainsKey(attributeName)
					? user.AdditionalData[attributeName]
					: null;
				Console.WriteLine($"{nameof(user.AdditionalData)}[{attributeName}]: {attributeValue}");
				Console.WriteLine($"Json:");
				Console.WriteLine(JsonSerializer.Serialize(user, _jsonSerializerOptions));
			}
		}

		// ユーザをカスタム属性付きでID指定で取得
		private async Task GetUserByIdAsync(IGraphServiceClient client, string extensionAppClientId) {
			var helper = new CustomAttributeHelper(extensionAppClientId);

			// カスタム属性名
			var attributeName = helper.GetFullName("TestNumber");

			var select = string.Join(',', new[] {
				"id",
				"surname",
				"givenName",
				"mail",
				attributeName
			});

			// ユーザを取得
			var id = "ca25b697-af90-4f5b-ae81-8eebc84acc24";
			var user = await client.Users[id]
				.Request()
				.Select(select)
				.GetAsync();

			Console.WriteLine($"{nameof(user.Id)}: {user.Id}");
			Console.WriteLine($"{nameof(user.Surname)}: {user.Surname}");
			Console.WriteLine($"{nameof(user.GivenName)}: {user.GivenName}");
			Console.WriteLine($"{nameof(user.Mail)}: {user.Mail}");
			// カスタム属性は存在しない場合がある
			var attributeValue = user.AdditionalData.ContainsKey(attributeName)
				? user.AdditionalData[attributeName]
				: null;
			Console.WriteLine($"{nameof(user.AdditionalData)}[{attributeName}]: {attributeValue}");
			Console.WriteLine($"Json:");
			Console.WriteLine(JsonSerializer.Serialize(user, _jsonSerializerOptions));
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
			//await GetUsersAsync(client);
			await GetUsersAsync(client, _config["ExtensionAppClientId"]);

			// ユーザーをID指定で取得
			await GetUserByIdAsync(client, _config["ExtensionAppClientId"]);
		}
	}
}
