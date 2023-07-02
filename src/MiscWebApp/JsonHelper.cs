using System.Text.Json;

namespace MiscWebApp;

public static class JsonHelper {
	public static readonly JsonSerializerOptions Options = new() {
		DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
	};
}
