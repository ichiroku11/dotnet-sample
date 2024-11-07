using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace AzureAdB2cGraphConsoleApp;

// サンプル
// 参考
// https://github.com/Azure-Samples/ms-identity-dotnetcore-b2c-account-management
// https://docs.microsoft.com/ja-jp/graph/sdks/choose-authentication-providers?tabs=CS#client-credentials-provider
public abstract class SampleBase(GraphServiceClient client, ILogger<SampleBase> logger) {
	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new() {
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			WriteIndented = true,
			Converters = {
				new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
			}
		};

	private readonly GraphServiceClient _client = client;
	private readonly ILogger _logger = logger;

	protected GraphServiceClient Client => _client;
	protected ILogger Logger => _logger;

	// JSON形式でログ出力
	protected void LogInformation<TValue>(TValue value) => _logger.LogInformation("{value}", JsonSerializer.Serialize(value, _jsonSerializerOptions));

	// サンプルの実行
	protected abstract Task RunCoreAsync();

	// サンプルの実行
	public async Task RunAsync() {
		await RunCoreAsync();
	}
}
