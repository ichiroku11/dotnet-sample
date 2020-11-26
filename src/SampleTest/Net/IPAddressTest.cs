using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

		[Theory]
		[InlineData("127.0.0.1")]
		[InlineData("0.0.0.0")]
		[InlineData("255.255.255.255")]
		public void AddressFamily_IPv4かどうかを判定する(string address) {
			// Arrange
			// Act
			// Assert
			Assert.Equal(AddressFamily.InterNetwork, IPAddress.Parse(address).AddressFamily);
		}

		[Theory]
		[MemberData(nameof(GetTestDataForToString))]
		public void ToString_文字列表現を確認する(IPAddress address, string expected) {
			// Arrange
			// Act
			var actual = address.ToString();

			// Assert
			Assert.Equal(expected: expected, actual);
		}
	}
}
