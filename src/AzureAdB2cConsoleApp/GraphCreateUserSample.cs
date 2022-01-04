using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace AzureAdB2cConsoleApp;

// ユーザーを作成
public class GraphCreateUserSample : GraphSampleBase {
	public GraphCreateUserSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);
		var attributeValue = new Random().Next(1, 10);

		var userToAdd = new User {
			// todo:
			GivenName = "",
			Surname = "",
			AdditionalData = new Dictionary<string, object> {
				[attributeName] = attributeValue,
			},
			Identities = new[] {
				new ObjectIdentity {
					Issuer = TenantId,
					// todo:
					IssuerAssignedId = "",
					SignInType = "emailAddress",
				},
			},
			PasswordProfile = new() {
				// 次回のログインでパスワードを変更する
				ForceChangePasswordNextSignIn = true,
				// todo:
				Password = "",
			},
		};
		var userAdded = await client.Users
			.Request()
			.AddAsync(userToAdd);

		ShowUser(userAdded);
	}
}
