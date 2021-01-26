using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NamedPipeConsoleApp {
	public static class StreamExtensions {
		private static readonly Encoding _encoding = Encoding.UTF8;

		private static readonly JsonSerializerOptions _options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			PropertyNameCaseInsensitive = true,
		};

		/// <summary>
		/// オブジェクトをJSON文字列として<see cref="Stream"/>に書き込む
		/// </summary>
		/// <typeparam name="TObject"></typeparam>
		/// <param name="stream"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static async Task WriteAsJsonAsync<TObject>(this Stream stream, TObject obj) {
			var json = JsonSerializer.Serialize(obj, _options);

			using var writer = new StreamWriter(stream, _encoding, leaveOpen: true);
			// 1行で書き込む
			await writer.WriteLineAsync(json);
		}

		/// <summary>
		/// <see cref="Stream"/>からJSON文字列をオブジェクトとして読み込む
		/// </summary>
		/// <typeparam name="TObject"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static async Task<TObject> ReadFromJsonAsync<TObject>(this Stream stream) {
			using var reader = new StreamReader(stream, _encoding, leaveOpen: true);
			// 1行を読み込む
			var json = await reader.ReadLineAsync();

			return JsonSerializer.Deserialize<TObject>(json, _options);
		}
	}
}
