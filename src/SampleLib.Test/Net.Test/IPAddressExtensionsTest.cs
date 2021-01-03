using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleLib.Net.Test {
	public class IPAddressExtensionsTest {
		public static IEnumerable<object[]> GetTestDataForIPv4LogicalAnd() {
			yield return new object[] {
				new IPAddress(new byte[] { 192, 168, 1, 1 }),
				new IPAddress(new byte[] { 255, 255, 0, 0 }),
				new IPAddress(new byte[] { 192, 168, 0, 0 }),
			};

			yield return new object[] {
				new IPAddress(new byte[] { 192, 168, 224, 1 }),
				new IPAddress(new byte[] { 255, 255, 192, 0 }),
				new IPAddress(new byte[] { 192, 168, 192, 0 }),
			};
		}

		[Theory]
		[MemberData(nameof(GetTestDataForIPv4LogicalAnd))]
		public void IPv4LogicalAnd_正しくAND演算できる(IPAddress address, IPAddress mask, IPAddress expected) {
			// Arrange
			// Act
			var actual = address.IPv4LogicalAnd(mask);

			// Assert
			Assert.True(actual.Equals(expected));
		}


		public static IEnumerable<object[]> GetTestDataForIsIPv4InSameSubnet() {
			// 同じサブネットに属する
			yield return new object[] {
				new IPAddress(new byte[] { 192, 168, 1, 1 }),
				new IPAddress(new byte[] { 192, 168, 0, 0 }),
				new IPAddress(new byte[] { 255, 255, 0, 0 }),
				true,
			};
			yield return new object[] {
				new IPAddress(new byte[] { 192, 168, 224, 1 }),
				new IPAddress(new byte[] { 192, 168, 192, 0 }),
				new IPAddress(new byte[] { 255, 255, 192, 0 }),
				true,
			};

			// 同じサブネットに属さない
			yield return new object[] {
				new IPAddress(new byte[] { 192, 168, 1, 1 }),
				new IPAddress(new byte[] { 192, 169, 0, 0 }),
				new IPAddress(new byte[] { 255, 255, 0, 0 }),
				false,
			};
			yield return new object[] {
				new IPAddress(new byte[] { 192, 168, 224, 1 }),
				new IPAddress(new byte[] { 192, 168, 128, 0 }),
				new IPAddress(new byte[] { 255, 255, 192, 0 }),
				false,
			};
		}

		[Theory]
		[MemberData(nameof(GetTestDataForIsIPv4InSameSubnet))]
		public void IsIPv4InSameSubnet_正しく判定できる(IPAddress address, IPAddress subnet, IPAddress mask, bool expected) {
			// Arrange
			// Act
			var actual = address.IsIPv4InSameSubnet(subnet, mask);

			// Assert
			Assert.Equal(expected, actual);
		}


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
