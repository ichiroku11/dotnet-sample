using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cConsoleApp;

// ユーザーを作成
public class GraphCreateUserSample : GraphSampleBase {
	public GraphCreateUserSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		var random = new Random();

		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);
		var attributeValue = random.Next(1, 10);

		var suffix = random.Next(1, 100).ToString();

		var userToAdd = new User {
			GivenName = $"太郎{suffix}",
			Surname = "テスト",
			AdditionalData = new Dictionary<string, object> {
				[attributeName] = attributeValue,
			},
			Identities = new[] {
				new ObjectIdentity {
					Issuer = TenantId,
					IssuerAssignedId = $"taro{suffix}@example.jp",
					SignInType = "emailAddress",
				},
			},
			PasswordProfile = new() {
				// 次回のログインでパスワードを変更する
				ForceChangePasswordNextSignIn = true,
				Password = PasswordHelper.Generate(6, 6, 4),
			},
		};
		var userAdded = await client.Users
			.Request()
			.AddAsync(userToAdd);

		ShowUser(userAdded);
	}
}
