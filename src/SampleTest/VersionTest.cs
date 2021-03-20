using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest {
	public class VersionTest {
		[Fact]
		public void Constructor_コンストラクタ引数に文字列を指定できる() {
			// Arrange
			// Act
			var version = new Version("1.2.3.4");

			// Assert
			Assert.Equal(1, version.Major);
			Assert.Equal(2, version.Minor);
			Assert.Equal(3, version.Build);
			Assert.Equal(4, version.Revision);
		}

		[Fact]
		public void Parse_文字列からインスタンスを生成できる() {
			// Arrange
			// Act
			var version = Version.Parse("1.2.3.4");

			// Assert
			Assert.Equal(1, version.Major);
			Assert.Equal(2, version.Minor);
			Assert.Equal(3, version.Build);
			Assert.Equal(4, version.Revision);
		}

		[Theory]
		// 左 == 右
		[InlineData("0.0.0.0", "0.0.0.0", true)]
		// 左 >= 右（各構成要素）
		[InlineData("0.0.0.2", "0.0.0.1", true)]
		[InlineData("0.0.2.0", "0.0.1.0", true)]
		[InlineData("0.2.0.0", "0.1.0.0", true)]
		[InlineData("2.0.0.0", "1.0.0.0", true)]
		public void OperatorGreaterThanOrEqual_左の値が右の値以上かどうかを判断する(string leftText, string rightText, bool expected) {
			// Arrange
			var left = new Version(leftText);
			var right = new Version(rightText);

			// Act
			var actual = left >= right;

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
