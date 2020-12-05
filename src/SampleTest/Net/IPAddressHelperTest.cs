using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Net {
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

		public static IEnumerable<object[]> GetTestDataForGetSubnetMask() {
			// todo:
			/*
			yield return new object[] { 0, new IPAddress(new byte[] { 0, 0, 0, 0 }) };
			yield return new object[] { 8, new IPAddress(new byte[] { 255, 0, 0, 0 }) };
			yield return new object[] { 16, new IPAddress(new byte[] { 255, 255, 0, 0 }) };
			yield return new object[] { 24, new IPAddress(new byte[] { 255, 255, 255, 0 }) };
			yield return new object[] { 32, new IPAddress(new byte[] { 255, 255, 255, 255 }) };
			*/

			yield return new object[] { 4, new IPAddress(new byte[] { 240, 0, 0, 0 }) };

			// todo:
			/*
			yield return new object[] { 12, new IPAddress(new byte[] { 255, 240, 0, 0 }) };
			yield return new object[] { 18, new IPAddress(new byte[] { 255, 255, 192, 0 }) };
			yield return new object[] { 25, new IPAddress(new byte[] { 255, 255, 255, 128 }) };
			*/
		}

		[Theory(Skip = "実装中")]
		[MemberData(nameof(GetTestDataForGetSubnetMask))]
		public void GetSubnetMask_サブネットマスクを取得できる(int prefix, IPAddress expected) {
			// Arrange
			// Act
			var actual = IPAddressHelper.GetSubnetMask(prefix);

			// Assert
			Assert.True(actual.Equals(expected));
		}
	}
}
