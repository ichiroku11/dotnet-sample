using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Extensions.FileSystemGlobbing {
	public class InMemoryDirectoryInfoTest {
		private readonly ITestOutputHelper _output;

		public InMemoryDirectoryInfoTest(ITestOutputHelper output) {
			_output = output;
		}

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

		[Fact]
		public void EnumerateFileSystemInfos_カレントディレクトリをルートに指定するとファイルを列挙できる() {
			// Arrange
			var root = Directory.GetCurrentDirectory();
			_output.WriteLine(root);

			var files = new List<string> {
				"readme.md"
			};
			var dirInfo = new InMemoryDirectoryInfo(root, files);

			// Act
			var fileInfos = dirInfo.EnumerateFileSystemInfos();

			// Assert
			var fileInfo = Assert.Single(fileInfos);
			_output.WriteLine(fileInfo.FullName);

			Assert.Equal("readme.md", fileInfo.Name);
		}

		[Fact]
		public void EnumerateFileSystemInfos_適当なフォルダをルートに指定するとファイルを列挙できる() {
			// Arrange
			var root = @"c:\temp";
			_output.WriteLine(root);

			var files = new List<string> {
				"readme.md"
			};
			var dirInfo = new InMemoryDirectoryInfo(root, files);

			// Act
			var fileInfos = dirInfo.EnumerateFileSystemInfos();

			// Assert
			Assert.Empty(fileInfos);
		}
	}
}
