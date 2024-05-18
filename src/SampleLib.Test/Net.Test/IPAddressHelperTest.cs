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

	public static TheoryData<int, string> GetTheoryDataForGetSubnetMask() {
		return new() {
			{ 0, "0.0.0.0" },
			{ 4, "240.0.0.0" },
			{ 8, "255.0.0.0" },
			{ 12, "255.240.0.0" },
			{ 16, "255.255.0.0" },
			{ 18, "255.255.192.0" },
			{ 24, "255.255.255.0" },
			{ 25, "255.255.255.128" },
			{ 32, "255.255.255.255" },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForGetSubnetMask))]
	public void GetSubnetMask_サブネットマスクを取得できる(int prefix, string expectedText) {
		// Arrange
		var expected = IPAddress.Parse(expectedText);

		// Act
		var actual = IPAddressHelper.GetSubnetMask(prefix);

		// Assert
		Assert.True(actual.Equals(expected));
	}
}
