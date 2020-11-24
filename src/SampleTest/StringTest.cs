using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	public class StringTest {
		[Fact]
		public void Equals_引数がnullでも比較できる() {
			// Arrange
			// Act
			// Assert
			Assert.False(string.Equals("", null, StringComparison.OrdinalIgnoreCase));
			Assert.False(string.Equals(null, "", StringComparison.OrdinalIgnoreCase));

			Assert.True(string.Equals(null, null, StringComparison.OrdinalIgnoreCase));
		}
	}
}
