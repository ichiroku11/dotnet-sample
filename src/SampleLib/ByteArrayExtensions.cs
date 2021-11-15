using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleLib;

public static class ByteArrayExtensions {
	// 16進数文字列を取得する
	public static string ToHexString(this byte[] bytes) {
		return BitConverter.ToString(bytes).ToLower().Replace("-", "");
	}
}
