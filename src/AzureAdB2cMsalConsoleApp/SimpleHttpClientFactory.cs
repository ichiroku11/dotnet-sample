using Microsoft.Identity.Client;

namespace AzureAdB2cMsalConsoleApp;

public class SimpleHttpClientFactory : IMsalHttpClientFactory {
	private readonly HttpClient _client;

	public SimpleHttpClientFactory(HttpClient client) {
		_client = client;
	}

	public HttpClient GetHttpClient() => _client;
}
