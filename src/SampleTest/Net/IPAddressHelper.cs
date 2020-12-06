using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest.Net {
	public class IPAddressHelper {
		// オクテットの値 => マスクのバイト
		private static readonly Dictionary<int, byte> _octetPrefixToByte = new() {
			[0] = 0b0000_0000, // 0
			[1] = 0b1000_0000, // 128
			[2] = 0b1100_0000, // 192
			[3] = 0b1110_0000, // 224
			[4] = 0b1111_0000, // 240
			[5] = 0b1111_1000, // 248
			[6] = 0b1111_1100, // 252
			[7] = 0b1111_1110, // 254
			[8] = 0b1111_1111, // 255
		};

		/// <summary>
		/// IPv4のサブネットマスクを取得
		/// </summary>
		/// <param name="prefix"></param>
		/// <returns></returns>
		public static IPAddress GetSubnetMask(int prefix) {
			if (prefix is < 0 or > 32) {
				throw new ArgumentOutOfRangeException(nameof(prefix));
			}

			// オクテットごとにプレフィックスを分解する
			// 例
			//  4 => 4 0 0 0
			//  8 => 8 0 0 0
			// 12 => 8 4 0 0
			// 16 => 8 8 0 0
			// 20 => 8 8 4 0
			// 24 => 8 8 8 0
			// 28 => 8 8 8 4
			// 32 => 8 8 8 8
			var prefixes = new[] {
				0, 0, 0, 0
			};
			foreach (var index in Enumerable.Range(0, prefix)) {
				prefixes[index / 8] += 1;
			}

			// オクテットごとにサブネットマスクのバイトに変換
			var bytes = prefixes.Select(value => _octetPrefixToByte[value]).ToArray();

			return new IPAddress(bytes);
		}
	}
}
