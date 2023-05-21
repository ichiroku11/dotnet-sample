using AzureAdB2cUserManagementConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

// ユーザーのサインインメールアドレスを変更する
internal class GraphUpdateUserSignInMailAddressSample : GraphSampleBase {
	public GraphUpdateUserSignInMailAddressSample(IConfiguration config, ILogger<GraphSampleBase> logger) : base(config, logger) {
	}

	protected override async Task RunCoreAsync(GraphServiceClient client) {
		// todo:
		await Task.CompletedTask;
	}
}
