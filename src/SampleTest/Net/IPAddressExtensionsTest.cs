using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Net {
	public class IPAddressExtensionsTest {
		public static IEnumerable<object[]> GetTestDataForIsIPv4Private() {
			// それぞれの範囲の境界値（含む）
			yield return new object[] { IPAddress.Parse("10.0.0.0"), true };
			yield return new object[] { IPAddress.Parse("10.255.255.255"), true };
			yield return new object[] { IPAddress.Parse("172.16.0.0"), true };
			yield return new object[] { IPAddress.Parse("172.31.255.255"), true };
			yield return new object[] { IPAddress.Parse("192.168.0.0"), true };
			yield return new object[] { IPAddress.Parse("192.168.255.255"), true };

			// それぞれの範囲内の値
			yield return new object[] { IPAddress.Parse("10.1.1.1"), true };
			yield return new object[] { IPAddress.Parse("172.17.1.1"), true };
			yield return new object[] { IPAddress.Parse("192.168.1.1"), true };

			// それぞれの範囲外の値
			yield return new object[] { IPAddress.Parse("9.255.255.255"), false };
			yield return new object[] { IPAddress.Parse("11.0.0.0"), false };
			yield return new object[] { IPAddress.Parse("172.15.255.255"), false };
			yield return new object[] { IPAddress.Parse("172.32.0.0"), false };
			yield return new object[] { IPAddress.Parse("192.167.255.255"), false };
			yield return new object[] { IPAddress.Parse("192.169.0.0"), false };
		}

		[Theory]
		[MemberData(nameof(GetTestDataForIsIPv4Private))]
		public void IsIPv4Private_プライベートIPアドレスを正しく判定できる(IPAddress address, bool expected) {
			// Arrange
			// Act
			// Assert
			Assert.Equal(expected, address.IsIPv4Private());
		}
	}
}
