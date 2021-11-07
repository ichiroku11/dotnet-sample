using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Extensions.FileSystemGlobbing {
	public class InMemoryDirectoryInfoTest {
		[Fact]
		public void FullName_フォルダのフルパスを確認できる() {
			// Arrange
			var dirInfo = new InMemoryDirectoryInfo(@"c:\temp", null);

			// Act
			var fullName = dirInfo.FullName;

			// Assert
			Assert.Equal(@"c:\temp", fullName);
		}

		[Fact]
		public void Name_フォルダ名を確認する() {
			// Arrange
			var dirInfo = new InMemoryDirectoryInfo(@"c:\temp", null);

			// Act
			var name = dirInfo.Name;

			// Assert
			Assert.Equal(@"temp", name);
		}
	}
}
