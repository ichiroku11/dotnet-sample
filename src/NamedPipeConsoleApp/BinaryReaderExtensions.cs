using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NamedPipeConsoleApp {
	// BinaryReaderの拡張メソッドを定義
	public static class BinaryReaderExtensions {
		// オブジェクトの読み込み
		public static TObject ReadObject<TObject>(this BinaryReader reader) {
			// 長さを読み込んでから、バイト配列を読み込む
			var length = reader.ReadInt32();
			var bytes = reader.ReadBytes(length);

			return new ObjectConverter<TObject>().FromByteArray(bytes);
		}
	}
}
