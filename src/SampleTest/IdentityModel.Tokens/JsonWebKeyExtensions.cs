using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SampleTest.IdentityModel.Tokens;

internal static class JsonWebKeyExtensions {
	internal static string ToJson(this JsonWebKey key) {
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			// nullをシリアライズしない
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver {
				Modifiers = {
					// 空配列をシリアライズしないため
					JsonTypeInfoModifiers.ReturnNullIfCollectionEmpty
				},
			}
		};

		return JsonSerializer.Serialize(key, options);
	}

}
