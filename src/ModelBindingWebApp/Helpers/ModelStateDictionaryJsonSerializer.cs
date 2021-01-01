using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Helpers {
	// ModelStateDictionaryのJsonSerializer
	public static class ModelStateDictionaryJsonSerializer {
		// JSONシリアライズ用のオブジェクト
		private class JsonEntry {
			public static JsonEntry Create(string key, ModelStateEntry entry)
				=> new JsonEntry {
					Key = key,
					RawValues = entry.RawValue switch {
						null => new string[0],
						string rawValue => new[] { rawValue },
						string[] rawValues => rawValues,
						_ => throw new InvalidOperationException(),
					},
					AttemptedValue = entry.AttemptedValue,
					ErrorMessages = entry.Errors.Select(error => error.ErrorMessage),
				};

			public string Key { get; set; }

			// ModelStateEntry.RawValueはおそらくstringかstring[]になる
			// JSONでstringとstring[]の2パターンの読み込みが難しそうなのでstring[]として扱う
			public string[] RawValues { get; set; }

			public string AttemptedValue { get; set; }

			public IEnumerable<string> ErrorMessages { get; set; }

			[JsonIgnore]
			public object RawValue => RawValues.Length switch {
				0 => null,
				1 => RawValues[0],
				_ => RawValues,
			};
		}

		private static JsonSerializerOptions _jsonSerializerOptions
			= new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				PropertyNameCaseInsensitive = true,
			};

		// JSON文字列にシリアライズ
		public static string Serialize(ModelStateDictionary modelStates) {
			var entries = modelStates.Select(entry => JsonEntry.Create(entry.Key, entry.Value));
			return JsonSerializer.Serialize(entries, _jsonSerializerOptions);
		}

		// JSON文字列をデシリアライズ
		public static ModelStateDictionary Deserialize(string json) {
			var modelStates = new ModelStateDictionary();

			var entries = JsonSerializer.Deserialize<JsonEntry[]>(json, _jsonSerializerOptions);
			foreach (var entry in entries) {
				modelStates.SetModelValue(entry.Key, entry.RawValue, entry.AttemptedValue);
				foreach (var errorMessage in entry.ErrorMessages) {
					modelStates.AddModelError(entry.Key, errorMessage);
				}
			}

			return modelStates;
		}
	}
}
