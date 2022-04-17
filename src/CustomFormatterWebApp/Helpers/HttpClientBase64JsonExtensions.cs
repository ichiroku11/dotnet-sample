using System.Net.Http.Headers;

namespace CustomFormatterWebApp.Helpers;

public static class HttpClientBase64JsonExtensions {
	public static Task<HttpResponseMessage> PostAsBase64JsonAsync<TValue>(
		this HttpClient client,
		string? requestUri,
		TValue value) {
		var base64json = Base64JsonSerializer.Serialize(value);
		var content = new StringContent(base64json);
		return client.PostAsync(requestUri, content);
	}
}
