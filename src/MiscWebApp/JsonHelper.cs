using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiscWebApp;

public static class JsonHelper {
	public static readonly JsonSerializerOptions Options = new() {
		DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		Converters = {
			// enumを文字列として扱う
			new JsonStringEnumConverter(),
		}
	};
}
