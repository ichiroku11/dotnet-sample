using System.Text;
using System.Text.Json;

namespace CustomFormatterWebApp.Formatters;

public static class Base64JsonSerializer {
	private static readonly JsonSerializerOptions _options
		= new () {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	public static TValue? Deserialize<TValue>(string base64json) {
		var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64json));
		return JsonSerializer.Deserialize<TValue>(json, _options);
	}

	public static string Serialize<TValue>(TValue value) {
		var json = JsonSerializer.Serialize(value, _options);
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
	}
}
