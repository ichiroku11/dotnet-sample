using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApi;

public abstract class ControllerTestBase(
	ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: IClassFixture<WebApplicationFactory<Program>> {

	private readonly ITestOutputHelper _output = output;
	private readonly WebApplicationFactory<Program> _factory = factory;

	private static readonly JsonSerializerOptions _jsonSerializerOptions
		= new() {
			PropertyNameCaseInsensitive = true,
		};

	protected static StringContent GetJsonStringContent<TModel>(TModel model) {
		var json = JsonSerializer.Serialize(model, _jsonSerializerOptions);
		var content = new StringContent(json);
		return content;
	}

	protected static async Task<TModel?> DeserializeAsync<TModel>(HttpResponseMessage response) {
		var json = await response.Content.ReadAsStringAsync();
		var model = JsonSerializer.Deserialize<TModel>(json, _jsonSerializerOptions);
		return model;
	}

	protected void WriteLine(string message) => _output.WriteLine(message);

	protected HttpClient CreateClient() {
		return _factory.CreateDefaultClient(new LoggingHandler(_output));
	}

	// テスト用のユーザーで認証する
	protected HttpClient CreateClientWithTestAuth() {
		var client = _factory
			// https://docs.microsoft.com/ja-jp/aspnet/core/test/integration-tests?view=aspnetcore-6.0
			.WithWebHostBuilder(builder => {
				builder.ConfigureServices(services => {
					services
						.AddAuthentication(_testAuthScheme)
						.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(_testAuthScheme, _ => { }); ;
				});
			})
			.CreateDefaultClient(new LoggingHandler(_output));
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_testAuthScheme);

		return client;
	}

	private class LoggingHandler(ITestOutputHelper output) : DelegatingHandler {
		private readonly ITestOutputHelper _output = output;

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			_output.WriteLine(request.ToString());
			if (request.Content != null) {
				_output.WriteLine(await request.Content.ReadAsStringAsync(cancellationToken));
			}

			var response = await base.SendAsync(request, cancellationToken);

			_output.WriteLine(response.ToString());
			if (response.Content != null) {
				_output.WriteLine(await response.Content.ReadAsStringAsync(cancellationToken));
			}
			return response;

		}
	}

	private const string _testAuthScheme = "Test";

	private class TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
		: AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder) {
		protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
			// テスト用のユーザーでログインしたことにする
			var claims = new[] {
				new Claim(ClaimTypes.NameIdentifier, "11"),
				new Claim(ClaimTypes.Name, "xx"),
			};
			var identity = new ClaimsIdentity(claims, _testAuthScheme);
			var principal = new ClaimsPrincipal(identity);
			var ticket = new AuthenticationTicket(principal, _testAuthScheme);
			var result = AuthenticateResult.Success(ticket);
			return Task.FromResult(result);
		}
	}
}
