using System.Net;
using Xunit;

namespace SampleLib.Net.Test;

public class IPAddressExtensionsTest {
	public static TheoryData<byte[], byte[], byte[]> GetTheoryData_IPv4LogicalAnd() {
		return new() {
			{
				new byte[] { 192, 168, 1, 1 },
				new byte[] { 255, 255, 0, 0 },
				new byte[] { 192, 168, 0, 0 }
			},
			{
				new byte[] { 192, 168, 224, 1 },
				new byte[] { 255, 255, 192, 0 },
				new byte[] { 192, 168, 192, 0 }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_IPv4LogicalAnd))]
	public void IPv4LogicalAnd_正しくAND演算できる(byte[] addressBytes, byte[] maskBytes, byte[] expectedBytes) {
		// Arrange
		var address = new IPAddress(addressBytes);
		var mask = new IPAddress(maskBytes);
		var expected = new IPAddress(expectedBytes);

		// Act
		var actual = address.IPv4LogicalAnd(mask);

		// Assert
		Assert.True(actual.Equals(expected));
	}

	public static TheoryData<byte[], byte[], byte[], bool> GetTheoryData_IsIPv4InSameSubnet() {
		return new() {
			// 同じサブネットに属する
			{
				new byte[] { 192, 168, 1, 1 },
				new byte[] { 192, 168, 0, 0 },
				new byte[] { 255, 255, 0, 0 },
				true
			},
			{
				new byte[] { 192, 168, 224, 1 },
				new byte[] { 192, 168, 192, 0 },
				new byte[] { 255, 255, 192, 0 },
				true
			},
			// 同じサブネットに属さない
			{
				new byte[] { 192, 168, 1, 1 },
				new byte[] { 192, 169, 0, 0 },
				new byte[] { 255, 255, 0, 0 },
				false
			},
			{
				new byte[] { 192, 168, 224, 1 },
				new byte[] { 192, 168, 128, 0 },
				new byte[] { 255, 255, 192, 0 },
				false
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_IsIPv4InSameSubnet))]
	public void IsIPv4InSameSubnet_正しく判定できる(byte[] addressBytes, byte[] subnetBytes, byte[] maskBytes, bool expected) {
		// Arrange
		var address = new IPAddress(addressBytes);
		var subnet = new IPAddress(subnetBytes);
		var mask = new IPAddress(maskBytes);


		// Act
		var actual = address.IsIPv4InSameSubnet(subnet, mask);

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<string, bool> GetTheoryData_IsIPv4Private() {
		return new() {
			// それぞれの範囲の境界値（含む）
			{ "10.0.0.0", true },
			{ "10.255.255.255", true },
			{ "172.16.0.0", true },
			{ "172.31.255.255", true },
			{ "192.168.0.0", true },
			{ "192.168.255.255", true },

			// それぞれの範囲内の値
			{ "10.1.1.1", true },
			{ "172.17.1.1", true },
			{ "192.168.1.1", true },

			// それぞれの範囲外の値
			{ "9.255.255.255", false },
			{ "11.0.0.0", false },
			{ "172.15.255.255", false },
			{ "172.32.0.0", false },
			{ "192.167.255.255", false },
			{ "192.169.0.0", false },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_IsIPv4Private))]
	public void IsIPv4Private_プライベートIPアドレスを正しく判定できる(string addressText, bool expected) {
		// Arrange
		var address = IPAddress.Parse(addressText);

		// Act
		var actual = address.IsIPv4Private();

		// Assert
		Assert.Equal(expected, actual);
	}
}
