using System.Net;
using Xunit;

namespace SampleLib.Net.Test;

public class IPAddressHelperTest {
	[Theory]
	[InlineData(-1)]
	[InlineData(33)]
	public void GetSubnetMask_引数が範囲外の場合は例外がスローされる(int prefix) {
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentOutOfRangeException>(() => {
			IPAddressHelper.GetSubnetMask(prefix);
		});
	}

	public static TheoryData<int, IPAddress> GetTheoryDataForGetSubnetMask() {
		return new() {
			{ 0, new IPAddress(new byte[] { 0, 0, 0, 0 }) },
			{ 4, new IPAddress(new byte[] { 240, 0, 0, 0 }) },
			{ 8, new IPAddress(new byte[] { 255, 0, 0, 0 }) },
			{ 12, new IPAddress(new byte[] { 255, 240, 0, 0 }) },
			{ 16, new IPAddress(new byte[] { 255, 255, 0, 0 }) },
			{ 18, new IPAddress(new byte[] { 255, 255, 192, 0 }) },
			{ 24, new IPAddress(new byte[] { 255, 255, 255, 0 }) },
			{ 25, new IPAddress(new byte[] { 255, 255, 255, 128 }) },
			{ 32, new IPAddress(new byte[] { 255, 255, 255, 255 }) },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForGetSubnetMask))]
	public void GetSubnetMask_サブネットマスクを取得できる(int prefix, IPAddress expected) {
		// Arrange
		// Act
		var actual = IPAddressHelper.GetSubnetMask(prefix);

		// Assert
		Assert.True(actual.Equals(expected));
	}
}
