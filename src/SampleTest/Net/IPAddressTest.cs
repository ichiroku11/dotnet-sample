using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Net {
	public class IPAddressTest {
		public static IEnumerable<object[]> GetTestDataForToString() {
			yield return new object[] { IPAddress.Any, "0.0.0.0" };
			yield return new object[] { IPAddress.Broadcast, "255.255.255.255" };
			yield return new object[] { IPAddress.Loopback, "127.0.0.1" };
			yield return new object[] { IPAddress.None, "255.255.255.255" };
		}

		[MemberData(nameof(GetTestDataForToString))]
		[Theory]
		public void ToString_文字列表現を確認する(IPAddress address, string expected) {
			// Arrange
			// Act
			var actual = address.ToString();

			// Assert
			Assert.Equal(expected: expected, actual);
		}
	}
}
