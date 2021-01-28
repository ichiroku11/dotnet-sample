using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TcpConsoleApp {
	// オブジェクトとバイト配列の変換
	public class ObjectConverter<TObject> {
		private readonly IFormatter _formatter;

		public ObjectConverter(IFormatter formatter = null) {
			_formatter = formatter ?? new BinaryFormatter();
		}

		// オブジェクト=>バイト配列
		public byte[] ToByteArray(TObject obj) {
			using (var stream = new MemoryStream()) {
				_formatter.Serialize(stream, obj);
				return stream.ToArray();
			}
		}

		// バイト配列=>オブジェクト
		public TObject FromByteArray(byte[] bytes) {
			using (var stream = new MemoryStream(bytes)) {
				return (TObject)_formatter.Deserialize(stream);
			}
		}
	}
}
