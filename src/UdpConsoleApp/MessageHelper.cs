using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UdpConsoleApp {
	/// <summary>
	/// 
	/// </summary>
	public class MessageHelper {
		private static readonly Encoding _encoding = Encoding.UTF8;

		private static readonly JsonSerializerOptions _options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			PropertyNameCaseInsensitive = true,
		};

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TMessage"></typeparam>
		/// <param name="message"></param>
		/// <returns></returns>
		public static byte[] ToByteArray<TMessage>(TMessage message) {
			var json = JsonSerializer.Serialize(message, _options);
			return _encoding.GetBytes(json);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TMessage"></typeparam>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static TMessage FromByteArray<TMessage>(byte[] bytes) {
			var json = _encoding.GetString(bytes);
			return JsonSerializer.Deserialize<TMessage>(json, _options);
		}
	}
}
