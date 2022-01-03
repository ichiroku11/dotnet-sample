namespace EndpointWebApp;

public static class HttpResponseExtensions {
	public static Task WriteLineAsync(this HttpResponse response, string text)
		=> response.WriteAsync($"{text}{Environment.NewLine}");
}
