using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
				WriteIndented = true,
			};

		private readonly IConfiguration _config;
		private readonly ILogger _logger;

		// Graph APIを呼び出すアプリ（資格情報などを管理する）
		private readonly IConfidentialClientApplication _confidentialClientApp;
		// カスタム属性の名前のヘルパー
		private readonly CustomAttributeHelper _customAttributeHelper;

		public GraphSample(IConfiguration config, ILogger<GraphSample> logger) {
			_config = config;
			_logger = logger;

			_confidentialClientApp = ConfidentialClientApplicationBuilder
				.Create(_config["ClientId"])
				.WithTenantId(_config["TenantId"])
				.WithClientSecret(_config["ClientSecret"])
				.Build();
			_customAttributeHelper = new CustomAttributeHelper(_config["ExtensionAppClientId"]);
		}

		// ユーザー情報をの表示
		private void ShowUser(User user) => Console.WriteLine(JsonSerializer.Serialize(user, _jsonSerializerOptions));

		// ユーザ一覧をカスタム属性付きで取得
		// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http
		// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management/blob/master/src/Services/UserService.cs
		private async Task GetUsersAsync(IGraphServiceClient client) {
			var attributeName = _customAttributeHelper.GetFullName("TestNumber");

			var select = string.Join(',', new[] {
				"id",
				"surname",
				"givenName",
				"identities",
				attributeName
			});

			// ユーザ一覧を取得
			var result = await client.Users
				.Request()
				.Select(select)
				/*
				// 取得するデータを式でも表現できるが、カスタム属性は取得できないのかも
				.Select(user => new {
					user.Id,
					user.Surname,
					user.GivenName,
					user.Identities,
				})
				*/
				// idが指定した値のどれか
				//.Filter("id in ('{id1}', '{id2}')")
				// surNameが指定した値ではじまる
				//.Filter("startsWith(surName, '{keyword}')")
				// surNameのcontainsどうもサポートしていない？
				//.Filter("contains(surName, '{keyword}')")
				.GetAsync();

			foreach (var user in result.CurrentPage) {
				ShowUser(user);
			}
		}

		// ユーザをカスタム属性付きでID指定で取得
		private async Task GetUserByIdAsync(IGraphServiceClient client) {
			var id = "{id}";

			var attributeName = _customAttributeHelper.GetFullName("TestNumber");

			var select = string.Join(',', new[] {
				"id",
				"surname",
				"givenName",
				"identities",
				attributeName
			});

			// ユーザをID指定で取得
			var user = await client.Users[id]
				.Request()
				.Select(select)
				.GetAsync();

			ShowUser(user);
		}

		// ユーザのカスタム属性を更新
		private async Task UpdateUserByIdAsync(IGraphServiceClient client) {
			var id = "{id}";

			var attributeName = _customAttributeHelper.GetFullName("TestNumber");
			var attributeValue = new Random().Next(1, 10);

			// 更新
			var userToUpdate = new User {
				AdditionalData = new Dictionary<string, object> {
					[attributeName] = attributeValue,
				},
			};
			await client.Users[id]
				.Request()
				.UpdateAsync(userToUpdate);

			// 取得して確認
			var userUpdated = await client.Users[id]
				.Request()
				.Select(attributeName)
				.GetAsync();
			ShowUser(userUpdated);
		}

		public async Task RunAsync() {
			// 認証プロバイダー
			// https://docs.microsoft.com/ja-jp/graph/sdks/choose-authentication-providers?tabs=CS#client-credentials-provider
			var clientCredentialProvider = new ClientCredentialProvider(_confidentialClientApp);

			// HTTPプロバイダー
			using var httpProvider = new LoggingHttpProvider(new HttpProvider(), _logger);

			// Graph APIを呼び出すクライアント
			var client = new GraphServiceClient(clientCredentialProvider, httpProvider);

			// ユーザ一覧を取得
			await GetUsersAsync(client);

			// ユーザーをID指定で取得
			//await GetUserByIdAsync(client);

			// ユーザーを更新
			//await UpdateUserByIdAsync(client);
		}
	}
}
