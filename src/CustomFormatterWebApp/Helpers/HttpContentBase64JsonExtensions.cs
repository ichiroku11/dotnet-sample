namespace CustomFormatterWebApp.Helpers;

public static class HttpContentBase64JsonExtensions {
	// base64にエンコードされたJSON文字列として読み込み
	public static async Task<TValue?> ReadFromBase64JsonAsync<TValue>(this HttpContent content) {
		var base64json = await content.ReadAsStringAsync();
		return Base64JsonSerializer.Deserialize<TValue>(base64json);
	}
}
