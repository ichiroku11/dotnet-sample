using Microsoft.AspNetCore.Http;

namespace SampleTest.AspNetCore.Http.HttpResults;

public static class HttpResponseExtensions {
	public static async Task<string> ReadBodyAsString(this HttpResponse response) {
		response.Body.Position = 0;

		using var reader = new StreamReader(response.Body);
		return await reader.ReadToEndAsync();
	}
}
