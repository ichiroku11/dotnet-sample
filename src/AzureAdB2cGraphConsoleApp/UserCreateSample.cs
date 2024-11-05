using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace AzureAdB2cGraphConsoleApp;

// ユーザーを作成
public class UserCreateSample(GraphServiceClient client, ILogger<SampleBase> logger, IOptions<GraphServiceOptions> options)
	: UserSampleBase(client, logger, options) {

	protected override async Task RunCoreAsync() {
		var random = new Random();

		var attributeName = GetCustomAttributeFullName(CustomAttributeNames.TestNumber);
		var attributeValue = random.Next(1, 10);

		var suffix = random.Next(1, 100).ToString();

		var givenName = $"太郎{suffix}";
		var surname = "テスト";

		var mail = $"taro{suffix}@example.jp";
		var password = PasswordHelper.Generate(6, 6, 4);

		var userToAdd = new User {
			// DisplayNameも必要（B2Cのユーザーフローでサインアップすると"unknown"になるのに）
			DisplayName = $"{surname} {givenName}",
			// 名
			GivenName = givenName,
			// 性
			Surname = surname,
			// カスタム属性
			AdditionalData = new Dictionary<string, object> {
				[attributeName] = attributeValue,
			},
			Identities = [
				// サインインするための情報
				// https://docs.microsoft.com/ja-jp/graph/api/resources/objectidentity
				new ObjectIdentity {
					Issuer = TenantId,
					IssuerAssignedId = mail,
					// メールアドレスでログインする
					SignInType = "emailAddress",
				},
			],
			PasswordProfile = new() {
				// 次回のログインでパスワードを変更する
				ForceChangePasswordNextSignIn = true,
				// パスワード
				Password = password,
			},
		};

		Logger.LogInformation("{mail}", mail);
		Logger.LogInformation("{password}", password);

		var userAdded = await Client.Users.PostAsync(userToAdd);

		if (userAdded is not null) {
			LogInformation(userAdded);
		}
	}
}
