using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NamedPipeConsoleApp {
	// BinaryWriterの拡張メソッドを定義
	public static class BinaryWriterExtensions {
		// オブジェクトの書き込み
		public static void WriteObject<TObject>(this BinaryWriter writer, TObject obj) {
			var bytes = new ObjectConverter<TObject>().ToByteArray(obj);

			// 長さを書き込んでからバイト配列を書き込む
			writer.Write(bytes.Length);
			writer.Write(bytes);
		}
	}
}
