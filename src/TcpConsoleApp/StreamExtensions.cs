using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TcpConsoleApp {
	public static class StreamExtensions {
		// オブジェクトの読み込み
		public static TObject ReadObject<TObject>(this Stream stream) {
			using (var reader = new BinaryReader(stream, Encoding.UTF8, true)) {
				// 長さを読み込んでから、バイト配列を読み込む
				var length = reader.ReadInt32();
				var bytes = reader.ReadBytes(length);

				var converter = new ObjectConverter<TObject>();
				return converter.FromByteArray(bytes);
			}
		}

		// オブジェクトの書き込み
		public static void WriteObject<TObject>(this Stream stream, TObject obj) {
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true)) {
				var converter = new ObjectConverter<TObject>();
				var bytes = converter.ToByteArray(obj);

				// 長さを書き込んでからバイト配列を書き込む
				writer.Write(bytes.Length);
				writer.Write(bytes);
			}
		}
	}
}
