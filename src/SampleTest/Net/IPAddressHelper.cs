using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest.Net {
	public class IPAddressHelper {

		public static IPAddress GetSubnetMask(int prefix) {
			if (prefix is < 0 or > 32) {
				throw new ArgumentOutOfRangeException(nameof(prefix));
			}

			// todo:
			return IPAddress.None;
		}
	}
}
