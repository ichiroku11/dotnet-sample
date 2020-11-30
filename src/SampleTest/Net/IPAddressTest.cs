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
		[Theory]
		[InlineData("127.0.0.1")]
		[InlineData("0.0.0.0")]
		[InlineData("192.168.0.0")]
		[InlineData("255.255.255.255")]
		public void AddressFamily_IPv4かどうかを判定する(string addressText) {
			// Arrange
			var address = IPAddress.Parse(addressText);

			// Act
			// Assert
			Assert.Equal(AddressFamily.InterNetwork, address.AddressFamily);
		}

		public static IEnumerable<object[]> GetTestDataForGetBytes() {
			yield return new object[] { IPAddress.Parse("0.0.0.0"), new byte[] { 0, 0, 0, 0 } };
			yield return new object[] { IPAddress.Parse("192.168.1.2"), new byte[] { 192, 168, 1, 2 } };
			yield return new object[] { new IPAddress(new byte[] { 192, 168, 1, 2 }), new byte[] { 192, 168, 1, 2 } };
		}

		[Theory]
		[MemberData(nameof(GetTestDataForGetBytes))]
		public void GetBytes_長さが4のIPアドレスを表すバイト配列を取得できる(IPAddress address, byte[] expected) {
			// Arrange
			// Act
			var actual = address.GetAddressBytes();

			// Assert
			Assert.Equal(expected, actual);
		}

		public static IEnumerable<object[]> GetTestDataForToString() {
			yield return new object[] { IPAddress.Any, "0.0.0.0" };
			yield return new object[] { IPAddress.Broadcast, "255.255.255.255" };
			yield return new object[] { IPAddress.Loopback, "127.0.0.1" };
			yield return new object[] { IPAddress.None, "255.255.255.255" };
		}

		[Theory]
		[MemberData(nameof(GetTestDataForToString))]
		public void ToString_文字列表現を確認する(IPAddress address, string expected) {
			// Arrange
			// Act
			var actual = address.ToString();

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
