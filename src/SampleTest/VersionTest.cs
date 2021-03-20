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
	}
}
