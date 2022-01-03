using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace AzureAdB2cConsoleApp;

// 参考
// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management
// https://docs.microsoft.com/ja-jp/graph/sdks/choose-authentication-providers?tabs=CS#client-credentials-provider

public class GraphSample {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new() {
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			WriteIndented = true,
		};

	private readonly IConfiguration _config;
	private readonly ILogger _logger;

	// カスタム属性の名前のヘルパー
	private readonly CustomAttributeHelper _customAttributeHelper;

	public GraphSample(IConfiguration config, ILogger<GraphSample> logger) {
		_config = config;
		_logger = logger;

		_customAttributeHelper = new CustomAttributeHelper(_config["ExtensionAppClientId"]);
	}

	// ユーザー情報をの表示
	private void ShowUser(User user) => Console.WriteLine(JsonSerializer.Serialize(user, _jsonSerializerOptions));

	// 関連リソース
	// https://docs.microsoft.com/ja-jp/graph/api/resources/user?view=graph-rest-1.0
	// https://docs.microsoft.com/ja-jp/graph/api/resources/objectidentity?view=graph-rest-1.0

	// ユーザ一覧をカスタム属性付きで取得
	// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http
	// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management/blob/master/src/Services/UserService.cs
	private async Task GetUsersAsync(GraphServiceClient client) {
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
			// 以下フィルタのサンプル
			// idが指定した値のどれか
			//.Filter("id in ('{id1}', '{id2}')")

			// surNameが指定した値ではじまる
			//.Filter("startsWith(surName, '{keyword}')")

			// surNameのcontainsどうもサポートしていない？
			//.Filter("contains(surName, '{keyword}')")

			// 指定したサインイン名でフィルタ
			// https://docs.microsoft.com/ja-jp/graph/api/user-list?view=graph-rest-1.0&tabs=http#example-2-get-a-user-account-using-a-sign-in-name
			//.Filter("identities/any(c:c/issuerAssignedId eq '{signInName}' and c/issuer eq '{tenant}')")
			// issuerとissuerAssignedIdどちらも指定する必要があるため、以下はエラー
			//.Filter("identities/any(c:c/issuerAssignedId eq '{signInName}')")

			// エラーになる
			//.Filter("identities/any(c:startsWith(c/issuerAssignedId, '{signInName}') and c/issuer eq '{tenant}')")

			.GetAsync();

		foreach (var user in result.CurrentPage) {
			ShowUser(user);
		}
	}

	// ユーザをカスタム属性付きでID指定で取得
	private async Task GetUserByIdAsync(GraphServiceClient client) {
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
	private async Task UpdateUserByIdAsync(GraphServiceClient client) {
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
		// クレデンシャル
		var credential = new ClientSecretCredential(
			tenantId: _config["TenantId"],
			clientId: _config["ClientId"],
			clientSecret: _config["ClientSecret"],
			options: new TokenCredentialOptions {
				AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
			});

		// スコープ
		var scopes = new[] { "https://graph.microsoft.com/.default" };

		// HTTPプロバイダー
		using var httpProvider = new LoggingHttpProvider(new HttpProvider(), _logger);

		// Graph APIを呼び出すクライアント
		var client = new GraphServiceClient(credential, scopes, httpProvider);

		// ユーザ一覧を取得
		await GetUsersAsync(client);

		// ユーザーをID指定で取得
		//await GetUserByIdAsync(client);

		// ユーザーを更新
		//await UpdateUserByIdAsync(client);
	}
}
