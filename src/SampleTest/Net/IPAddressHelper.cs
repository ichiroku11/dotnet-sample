using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest.Net {
	public class IPAddressHelper {
		/// <summary>
		/// IPv4のサブネットマスクを取得
		/// </summary>
		/// <param name="prefix"></param>
		/// <returns></returns>

		public static IPAddress GetSubnetMask(int prefix) {
			if (prefix is < 0 or > 32) {
				throw new ArgumentOutOfRangeException(nameof(prefix));
			}

			var bytes = new byte[4];

			return new IPAddress(bytes);
		}
	}
}
