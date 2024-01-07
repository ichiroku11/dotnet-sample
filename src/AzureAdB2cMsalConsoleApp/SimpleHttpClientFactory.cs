using Microsoft.Identity.Client;

namespace AzureAdB2cMsalConsoleApp;

public class SimpleHttpClientFactory(HttpClient client) : IMsalHttpClientFactory {
	private readonly HttpClient _client = client;

	public HttpClient GetHttpClient() => _client;
}
