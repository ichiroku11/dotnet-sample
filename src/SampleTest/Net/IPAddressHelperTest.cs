using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
