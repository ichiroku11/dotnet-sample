using System.Net;
using System.Net.Sockets;

namespace SampleTest.Net;

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

	[Fact]
	public void Equals_値を比較する() {
		// Arrange
		var address1 = new IPAddress(new byte[] { 192, 168, 0, 1 });
		var address2 = new IPAddress(new byte[] { 192, 168, 0, 1 });

		// Act
		// Assert
		Assert.False(ReferenceEquals(address1, address2));
		Assert.True(address1.Equals(address2));
	}

	public static TheoryData<IPAddress, byte[]> GetTheoryDataForGetBytes() {
		return new() {
			{
				IPAddress.Parse("0.0.0.0"),
				new byte[] { 0, 0, 0, 0 }
			},
			{
				// 10進数の並び順のバイト配列を取得できる
				IPAddress.Parse("192.168.1.2"),
				new byte[] { 192, 168, 1, 2 }
			},
			{
				new IPAddress(new byte[] { 192, 168, 1, 2 }),
				new byte[] { 192, 168, 1, 2 }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForGetBytes))]
	public void GetBytes_IPアドレスを表すバイト配列を取得できる(IPAddress address, byte[] expected) {
		// Arrange
		// Act
		var actual = address.GetAddressBytes();

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<IPAddress, string> GetTheoryDataForToString() {
		return new() {
			{ IPAddress.Any, "0.0.0.0" },
			{ IPAddress.Broadcast, "255.255.255.255" },
			{ IPAddress.Loopback, "127.0.0.1" },
			{ IPAddress.None, "255.255.255.255" },
		};
	}

	[Fact]
	public void OperatorEquality_値を比較しない() {
		// Arrange
		var address1 = new IPAddress(new byte[] { 192, 168, 0, 1 });
		var address2 = new IPAddress(new byte[] { 192, 168, 0, 1 });

		// Act
		// Assert
		Assert.False(address1 == address2);
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForToString))]
	public void ToString_文字列表現を確認する(IPAddress address, string expected) {
		// Arrange
		// Act
		var actual = address.ToString();

		// Assert
		Assert.Equal(expected, actual);
	}
}
