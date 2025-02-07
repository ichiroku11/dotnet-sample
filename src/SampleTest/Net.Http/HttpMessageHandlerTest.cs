using System.Net;

namespace SampleTest.Net.Http;

public class HttpMessageHandlerTest {
	// 何もしないハンドラ
	// HttpClientHandlerのかわり
	private class DoNothingHandler : HttpMessageHandler {
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			return Task.FromResult(new HttpResponseMessage {
				StatusCode = HttpStatusCode.OK,
				RequestMessage = request,
			});
		}
	}

	// 前後に処理をはさむハンドラ
	private class SampleHandler : DoNothingHandler {
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {

			// リクエストを送信する前に処理
			request.Headers.Add("X-SampleHandler", "Request");

			var response = await base.SendAsync(request, cancellationToken);

			// レスポンスを受信し後に処理
			response.Headers.Add("X-SampleHandler", "Response");

			return response;
		}
	}

	[Fact]
	public async Task SendAsync_オーバーライドして前後に処理をはさむ() {
		// Arrange
		using var client = new HttpClient(new SampleHandler());
		var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/");

		// Act
		var response = await client.SendAsync(request, CancellationToken.None);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal("Request", request.Headers.GetValues("X-SampleHandler").Single());
		Assert.Equal("Response", response.Headers.GetValues("X-SampleHandler").Single());
	}
}
